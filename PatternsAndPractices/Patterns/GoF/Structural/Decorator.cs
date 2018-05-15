using Xunit;

namespace PatternsAndPractices.Patterns.GoF.Structural
{
    public interface IDataStore
    {
        bool UpdateUserAddress(int userId, string address);
    }

    public class DataStore : IDataStore
    {
        public bool UpdateUserAddress(int userId, string address)
        {
            // Write data to DB
            return true;
        }
    }

    public abstract class DataStoreDecorator : IDataStore
    {
        protected readonly IDataStore _store;

        public DataStoreDecorator(IDataStore store) => _store = store;

        public virtual bool UpdateUserAddress(int userId, string address) => _store.UpdateUserAddress(userId, address);
    }

    public class AuditTrailDecorator : DataStoreDecorator
    {
        public AuditTrailDecorator(IDataStore store) : base(store)
        {
        }

        public override bool UpdateUserAddress(int userId, string address)
        {
            var result = base.UpdateUserAddress(userId, address);

            // Update Audit Trail

            return result;
        }
    }

    public class DataUpdatedNotificationDecorator : DataStoreDecorator
    {
        public DataUpdatedNotificationDecorator(IDataStore store) : base(store)
        {
        }

        public override bool UpdateUserAddress(int userId, string address)
        {
            var result = base.UpdateUserAddress(userId, address);

            // Send data to MessageBus

            return result;
        }
    }

    public class DecoratorTests
    {
        [Fact]
        public void Test()
        {
            IDataStore store = new DataStore();
            store = new AuditTrailDecorator(store);
            store = new DataUpdatedNotificationDecorator(store);

            store.UpdateUserAddress(34, "Street 10 A");
        }
    }
}