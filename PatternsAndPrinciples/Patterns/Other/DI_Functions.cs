using PatternsAndPinciples;
using PatternsAndPinciples.Patterns.Other;
using System;
using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace PatternsAndPrinciples.Patterns.Other
{
    /*
     * How to use functions instead of repository
     * 
     * Compare to the example in DI.cs
     */

    public class UserService
    {
        private readonly Func<int, User> _getUser;
        private readonly Func<User, bool> _updateUser;

        public UserService(Func<int, User> getUser, Func<User, bool> updateUser)
        {
            _getUser = getUser;
            _updateUser = updateUser;
        }

        public bool UpdateUser(int userId, string newValue)
        {
            var user = _getUser(userId);
            user.Value = newValue;
            return _updateUser(user);
        }
    }

    public class FunctionsFunctionalTest
    {
        public FunctionsFunctionalTest(ITestOutputHelper outputHelper) => Trace.Listeners.Add(new TestTraceListener(outputHelper));

        [Fact]
        public void TestFunc()
        {
            var get = new Func<int, User>(i => new User { Id = i, Value = "XXX" });
            var save = new Func<User, bool>(user => true);

            var service = new UserService(get, save);
            service.UpdateUser(1, "FF");

            // In "reality" would just use more functions instead of service class
            // NOTE: This is one of the reasons why C# would need better type inference
            var handle = new Func<int, string, Func<int, User>, Func<User, bool>, bool>((userId, newValue, getFunc, saveFunc) =>
            {
                var user = getFunc(userId);
                user.Value = newValue;
                return saveFunc(user);
            });

            handle(1, "FF", get, save);
        }

        [Fact]
        public void TestFuncWithRepo()
        {
            var repo = new UserRepository();

            var service = new UserService(repo.GetUser, repo.SaveUser);
            service.UpdateUser(1, "FF");
        }
    }
}