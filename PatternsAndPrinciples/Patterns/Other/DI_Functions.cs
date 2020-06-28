using System;
using Xunit;

namespace PatternsAndPrinciples.Patterns.Other.DI.Functions
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
        [Fact]
        public void TestFunc()
        {
            // "real" implementation would get data from DbContext
            var get = new Func<int, User>(i => new DBContext().Get<User>(i));
            // Test implementation would just return dummy data
            var getTest = new Func<int, User>(i => new User { Id = i, Value = "Test value" });

            var save = new Func<User, bool>(user => new DBContext().Save(user));

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
            var ctx = new DBContext();
            var repo = new UserRepository(ctx);

            var service = new UserService(repo.GetUser, repo.SaveUser);
            service.UpdateUser(1, "FF");
        }
    }
}