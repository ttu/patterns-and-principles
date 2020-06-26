using System;
using Xunit;

namespace PatternsAndPinciples.Patterns.Other
{
    public class Class_vs_Closure
    {
        public class NumberProvider
        {
            public int GetNumber() => 4;
        }

        public class Multiplier
        {
            private readonly NumberProvider _provider;

            public Multiplier(NumberProvider privider) => _provider = privider;

            public int Multiply(int value) => _provider.GetNumber() * value;
        }

        [Fact]
        public void TestWithClass()
        {
            var datastore = new NumberProvider();

            var multipler = new Multiplier(datastore);

            var result = HandleCalculations(multipler);

            Assert.Equal(17, result);
        }

        private int HandleCalculations(Multiplier multiplier)
        {
            var result = multiplier.Multiply(4);
            return result + 1;
        }

        // With Closure just wrap the NumberProvider inside the multiply function
        // This is pretty much same example as with DI_functions.cs but more simple

        [Fact]
        public void TestWithClosure()
        {
            var provider = new NumberProvider();

            var multiplyFunc = new Func<int, int>(value => provider.GetNumber() * value);

            var result = HandleCalculations(multiplyFunc);

            Assert.Equal(17, result);
        }

        private int HandleCalculations(Func<int, int> multiplyFunc)
        {
            var result = multiplyFunc(4);
            return result + 1;
        }
    }
}