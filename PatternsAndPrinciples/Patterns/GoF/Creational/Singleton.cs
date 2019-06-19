using System;
using Xunit;

namespace PatternsAndPinciples.Patterns.GoF.Creational
{
    public sealed class Singleton
    {
        private static Singleton _instance;

        private Singleton()
        {
        }

        public static Singleton Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Singleton();

                return _instance;
            }
        }
    }

    public sealed class ThreadSafeSingleton
    {
        private static readonly Lazy<ThreadSafeSingleton> Lazy =
            new Lazy<ThreadSafeSingleton>(new ThreadSafeSingleton());

        private ThreadSafeSingleton()
        {
        }

        public static ThreadSafeSingleton Instance => Lazy.Value;
    }

    public class SingletonTests
    {
        [Fact]
        public void Test()
        {
            var s1 = Singleton.Instance;
            var s2 = Singleton.Instance;

            Assert.True(ReferenceEquals(s1, s2));
        }

        [Fact]
        public void ThreadSafeTest()
        {
            var s1 = ThreadSafeSingleton.Instance;
            var s2 = ThreadSafeSingleton.Instance;

            Assert.True(ReferenceEquals(s1, s2));
        }
    }
}