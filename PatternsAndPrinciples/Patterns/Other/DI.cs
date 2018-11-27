using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace PatternsAndPinciples.Patterns.Other
{
    /*
     * Check example from Functions.cs - Traditional OOP section
     */

    public class User
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }

    public class UserRepository
    {
        public User GetUser(int userId) => new User { Id = userId, Value = "XXX" };

        public bool SaveUser(User use) => true;
    }

    public class UserServiceWithRepo
    {
        private readonly UserRepository _repo;

        public UserServiceWithRepo(UserRepository repo) => _repo = repo;

        public bool UpdateUser(int userId, string newValue)
        {
            var user = _repo.GetUser(userId);
            user.Value = newValue;
            return _repo.SaveUser(user);
        }
    }

    public class FunctionsTest
    {
        public FunctionsTest(ITestOutputHelper outputHelper) => Trace.Listeners.Add(new TestTraceListener(outputHelper));

        [Fact]
        public void TestWithRepo()
        {
            var repo = new UserRepository();

            var service = new UserServiceWithRepo(repo);
            service.UpdateUser(1, "FF");
        }
    }
}