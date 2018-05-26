namespace PatternsAndPractices.UML
{
    public class User { }

    public class BaseRepository<T>
    {
        public T Get()
        {
            return default(T);
        }
    }

    public class UserRepository : BaseRepository<User>
    {
    }
}