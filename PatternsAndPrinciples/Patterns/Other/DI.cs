using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace PatternsAndPinciples.Patterns.Other.DI
{
    /*
     * DI & Repository pattern
     * 
     * Compare to example DI_Functions.cs
     */

    public class User
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }

    // e.g. EF DbContext
    public class DBContext
    {
        // Example has only singe Get method that will return User with correct id
        public T Get<T>(int id) where T : class
        {
            return typeof(T) == typeof(User)
                ? new User { Id = id, Value = "XXX" } as T
                : default;
        }

        public bool Save<T>(T data) => true;
    }

    public class UserRepository
    {
        private readonly DBContext _context;

        public UserRepository(DBContext context) => context = _context;

        public User GetUser(int userId) => _context.Get<User>(userId);

        public bool SaveUser(User user) => _context.Save(user);
    }

    public class UserService
    {
        private readonly UserRepository _repo;

        public UserService(UserRepository repo) => _repo = repo;

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
            var ctx = new DBContext();

            var repo = new UserRepository(ctx);

            var service = new UserService(repo);
            service.UpdateUser(1, "FF");
        }
    }
}