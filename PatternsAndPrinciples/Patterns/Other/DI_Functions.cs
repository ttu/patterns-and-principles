using System;
using Xunit;

namespace PatternsAndPrinciples.Patterns.Other.DI.Functions
{
    /*
     * How to use functions instead of repository
     *
     * 1. Inject functions in constructor
     * 2. Pass functions as parameters
     * 3. Function composition
     * 
     * Compare to the UserService-class in DI.cs
     */

    // 1. Inject functions in constructor
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
    
    // Static class in here is just a collection of functions
    public static class UserServiceFuncs
    {
        // 2. Pass functions as parameters
        public static bool UpdateUserFunc(int userId, string newValue, Func<int, User> getUser, Func<User, bool> updateUser)
        {
            var user = getUser(userId);
            user.Value = newValue;
            return updateUser(user);
        }
        
        // 3. Function composition
        public static Func<int, string, bool> ComposeUpdateUserFunc(Func<int, User> getUser, Func<User, bool> updateUser)
        {
            return new Func<int, string, bool>((userId, newValue) =>
            {
                var user = getUser(userId);
                user.Value = newValue;
                return updateUser(user);
            });
        }
    }

    public class FunctionsFunctionalTest
    {
        [Fact]
        public void TestFunc()
        {
            var db = new DBContext();
            
            // "real" implementation would get data from DbContext
            var get = new Func<int, User>(i => db.Get<User>(i));
            // Test implementation would just return dummy data
            var getTest = new Func<int, User>(i => new User { Id = i, Value = "Test value" });

            var save = new Func<User, bool>(user => db.Save(user.Id, user));

            // 1. Inject functions in constructor
            var service = new UserService(get, save);
            service.UpdateUser(1, "FF");

            Assert.Equal("FF", db.Get<User>(1).Value);

            // In "reality" would just use more functions instead of service class

            // 2. Functions as parameters
            UserServiceFuncs.UpdateUserFunc(2, "FF2", get, save);

            Assert.Equal("FF2", db.Get<User>(2).Value);

            // 3. Function composition
            var updateUserComposed = UserServiceFuncs.ComposeUpdateUserFunc(get, save);
            updateUserComposed(3, "FF3");

            Assert.Equal("FF3", db.Get<User>(3).Value);

            // UpdateUser as Func
            // NOTE: This is one of the reasons why C# would need better type inference
            var updateUser = new Func<int, string, Func<int, User>, Func<User, bool>, bool>((userId, newValue, getFunc, saveFunc) =>
            {
                var user = getFunc(userId);
                user.Value = newValue;
                return saveFunc(user);
            });

            updateUser(4, "FF4", get, save);
            
            Assert.Equal("FF4", db.Get<User>(4).Value);
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