﻿using System.Diagnostics;
using Xunit;

namespace PatternsAndPrinciples.Patterns.GoF.Structural
{
    /*
     * Wraps an object to provider new behavior
     *
     * Example has a DataStore which saves data to db. Example has 2 decorators, which will
     * save the data to AuditTrail store and send a data update
     */

    public interface IDataStore
    {
        bool UpdateUserAddress(int userId, string address);
    }

    public class DataStore : IDataStore
    {
        public bool UpdateUserAddress(int userId, string address)
        {
            Trace.WriteLine($"Write data to DB: {userId} - {address}");

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

            Trace.WriteLine($"Update Audit Trail: {userId} - {address}");

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

            Trace.WriteLine($"Send data to MessageBus: {userId} - {address}");

            return result;
        }
    }

    public class MockDecorator : DataStoreDecorator
    {
        public MockDecorator(IDataStore store) : base(store)
        {
        }

        public override bool UpdateUserAddress(int userId, string address)
        {
            // Do not call base function so this is the only decorator executed
            Trace.WriteLine($"Do nothing: {userId} - {address}");
            return true;
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

        [Fact]
        public void Mock_Test()
        {
            IDataStore store = new DataStore();
            store = new MockDecorator(store);

            store.UpdateUserAddress(34, "Street 10 A");
        }
    }
}