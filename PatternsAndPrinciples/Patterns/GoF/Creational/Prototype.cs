﻿using Newtonsoft.Json;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Xunit;

namespace PatternsAndPrinciples.Patterns.GoF.Creational
{
    /*
     * A fully initialized instance to be copied or cloned
     */

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
            // TODO: Deep copy
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

        [Fact]
        public void AccountTest()
        {
            var account = new Account { Id = "my account" };

            var cloned = account.Clone() as Account;

            Assert.Equal(account.Id, cloned.Id);
        }

        [Fact]
        public void CloneTest()
        {
            var account = new Account { Id = "my account" };

            var cloned = CloneHelpers.CloneJson(account);

            Assert.Equal(account.Id, cloned.Id);
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
        public static T CloneJson<T>(T source)
        {
            var json = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static T Clone<T>(T source)
        {
            // This requires object to be marked as serialzable
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