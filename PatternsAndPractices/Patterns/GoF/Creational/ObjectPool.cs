using System;
using System.Collections.Generic;
using Xunit;

namespace PatternsAndPractices.Patterns.GoF.Creational
{
    public class Releasable
    {
        public Guid Id { get; } = Guid.NewGuid();
    }

    public class ReleasablePool
    {
        private readonly Stack<Releasable> _freeList = new Stack<Releasable>();

        public Releasable AquireReusable()
        {
            _freeList.TryPop(out var connection);
            return connection ?? new Releasable();
        }

        public void ReleaseReusable(Releasable toRelease)
        {
            _freeList.Push(toRelease);
        }
    }

    public class ObjectPoolTest
    {
        [Fact]
        public void ReleasableTest()
        {
            var pool = new ReleasablePool();

            var releasable1 = pool.AquireReusable();
            var releasable2 = pool.AquireReusable();
            Assert.NotEqual(releasable1.Id, releasable2.Id);

            var id1 = releasable1.Id;
            pool.ReleaseReusable(releasable1);

            var releasable3 = pool.AquireReusable();
            Assert.Equal(id1, releasable3.Id);
        }

        [Fact]
        public void Test()
        {
            var pool = new DataBaseConnection();

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
        private readonly Stack<DbConnection> _freeList;

        public DbConnection(Stack<DbConnection> freeList) => _freeList = freeList;

        public Guid Id { get; } = Guid.NewGuid();

        public void Dispose()
        {
            _freeList.Push(this);
        }
    }

    public class DataBaseConnection
    {
        private readonly Stack<DbConnection> _freeList = new Stack<DbConnection>();

        public DbConnection CreateConnection()
        {
            _freeList.TryPop(out var connection);
            return connection ?? new DbConnection(_freeList);
        }
    }
}