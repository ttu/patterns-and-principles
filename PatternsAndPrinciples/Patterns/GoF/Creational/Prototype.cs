using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Xunit;

namespace PatternsAndPinciples.Patterns.GoF.Creational
{
    public abstract class Prototype
    {
        public abstract Prototype Clone();
    }

    public class User : Prototype
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override Prototype Clone()
        {
            return (Prototype)this.MemberwiseClone();
        }
    }

    public class PrototypeTest
    {
        [Fact]
        public void Test()
        {
            var myUser = new User { Id = 1, Name = "Timmy" };

            var cloned = myUser.Clone() as User;

            Assert.Equal(myUser.Name, cloned.Name);
        }
    }

    // C# has a ICloneable interface
    public class Account : ICloneable
    {
        public string Id { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    // More common way is just to use generic clone helpers etc.
    public static class CloneHelpers
    {
        public static T Clone<T>(T source)
        {
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}