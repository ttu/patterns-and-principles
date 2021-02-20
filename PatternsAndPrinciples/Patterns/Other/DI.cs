using System.Collections.Generic;
using Xunit;

namespace PatternsAndPrinciples.Patterns.Other.DI
{
    /*
     * DI & Repository pattern
     *
     * Compare to example DI_Functions.cs
     */

    public class UserRepository
    {
        private readonly DBContext _context;

        public UserRepository(DBContext context) => _context = context;

        public User GetUser(int userId) => _context.Get<User>(userId);

        public bool SaveUser(User user) => _context.Save(user.Id, user);
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

    public class DITest
    {
        [Fact]
        public void TestWithRepo()
        {
            var ctx = new DBContext();

            var repo = new UserRepository(ctx);

            var service = new UserService(repo);
            service.UpdateUser(1, "FF");

            Assert.Equal("FF", repo.GetUser(1).Value);
        }
    }
    
    #region "Helpers & Models"
    
    public class User
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }

    // e.g. EF DbContext
    public class DBContext
    {
        private Dictionary<int, dynamic> _db = new Dictionary<int, dynamic>();
        
        // Example has only singe Get method that will return User with correct id
        public T Get<T>(int id) where T : class
        {
            T CreateNew(int newId) => typeof(T) == typeof(User)
                ? new User { Id = newId, Value = "XXX" } as T
                : default;

            if (_db.ContainsKey(id)) return _db[id];

            var newItem = CreateNew(id);
            _db.Add(id, newItem);
            return newItem;
        }

        public bool Save<T>(int id, T data) {
            _db[id] = data;
            return true;
        }
    }
    
    #endregion
}