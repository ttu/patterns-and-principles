using System;
using System.Collections.Generic;
using Xunit;

namespace PatternsAndPinciples.Patterns.GoF.Creational
{
    /*
     * Avoids expensive acquisition and release of resources by recycling objects that are no longer in use
     * 
     * 1) Pool with Aquire/Relese-methods
     * 2) Pool with automatic release on connection dispose
     */

    public class Connection
    {
        public Guid Id { get; } = Guid.NewGuid();
    }

    public class ConnectionPool
    {
        private readonly Stack<Connection> _freeList = new Stack<Connection>();

        public Connection AquireReusable()
        {
            _freeList.TryPop(out var connection);
            return connection ?? new Connection();
        }

        public void ReleaseReusable(Connection toRelease)
        {
            _freeList.Push(toRelease);
        }
    }

    public class ObjectPoolTest
    {
        [Fact]
        public void Releasable_Test()
        {
            var pool = new ConnectionPool();

            var releasable1 = pool.AquireReusable();
            var releasable2 = pool.AquireReusable();
            Assert.NotEqual(releasable1.Id, releasable2.Id);

            var id1 = releasable1.Id;
            pool.ReleaseReusable(releasable1);

            var releasable3 = pool.AquireReusable();
            Assert.Equal(id1, releasable3.Id);
        }

        [Fact]
        public void Disposable_Test()
        {
            var pool = new ConnectionManager();

            var con1 = pool.CreateConnection();
            var con2 = pool.CreateConnection();
            Assert.NotEqual(con1.Id, con2.Id);

            var id1 = con1.Id;
            con1.Dispose();

            var con3 = pool.CreateConnection();
            Assert.Equal(id1, con3.Id);
        }
    }

    public class DbConnection : IDisposable
    {
        private readonly Action<DbConnection> _releaseAction;

        public DbConnection(Action<DbConnection> releaseAction) => _releaseAction = releaseAction;

        public Guid Id { get; } = Guid.NewGuid();

        public void Dispose()
        {
            _releaseAction(this);
        }
    }

    public class ConnectionManager
    {
        private readonly Stack<DbConnection> _freeList = new Stack<DbConnection>();

        public DbConnection CreateConnection()
        {
            _freeList.TryPop(out var connection);
            return connection ?? new DbConnection((s) => ReleaseConnection(s));
        }

        private void ReleaseConnection(DbConnection connection)
        {
            _freeList.Push(connection);
        }
    }
}