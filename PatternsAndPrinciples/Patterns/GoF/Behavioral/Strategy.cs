using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Xunit;

namespace PatternsAndPrinciples.Patterns.GoF.Behavioral
{
    /*
     * Allows one of a family of algorithms to be selected on-the-fly at runtime.
     *
     * Example decides wheter return data in json or xml
     */

    public abstract class Strategy
    {
        public abstract string Serialize<T>(T data);
    }

    public class JsonStrategy : Strategy
    {
        public override string Serialize<T>(T data)
        {
            return JsonConvert.SerializeObject(data);
        }
    }

    public class XmlStrategy : Strategy
    {
        public override string Serialize<T>(T data)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));

            using (var textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, data);
                return textWriter.ToString();
            }
        }
    }

    public class DataParser
    {
        public Strategy Serializer { get; set; }

        public string SerializeData<T>(T data)
        {
            return Serializer.Serialize(data);
        }
    }

    public class StrategyTests
    {
        [Fact]
        public void SerializeTests()
        {
            var parser = new DataParser();

            var xmlStrategy = new XmlStrategy();
            var jsonStrategy = new JsonStrategy();

            parser.Serializer = jsonStrategy;

            var data = new User { Id = 2, Name = "Test user" };

            var json = parser.SerializeData(data);

            parser.Serializer = xmlStrategy;

            var xml = parser.SerializeData(data);
        }

        public class User
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public enum SerializerType
        {
            Xml,
            Json
        }

        // "Strategy" example with using functions 

        public class SerializerDictHelper
        {
            private Dictionary<SerializerType, Func<dynamic, string>> _funcs = new Dictionary<SerializerType, Func<dynamic, string>>();

            public SerializerDictHelper()
            {
                _funcs.Add(SerializerType.Xml, SerializeXml);
                _funcs.Add(SerializerType.Json, SerializeJson);
            }

            private string SerializeXml(dynamic data)
            {
                var xmlSerializer = new XmlSerializer(data.GetType());

                using (StringWriter textWriter = new StringWriter())
                {
                    xmlSerializer.Serialize(textWriter, data);
                    return textWriter.ToString();
                }
            }

            private string SerializeJson(dynamic data)
            {
                return JsonConvert.SerializeObject(data);
            }

            public string SerializeData(SerializerType type, dynamic data)
            {
                return _funcs[type](data);
            }
        }

        public class SerializeHelper
        {
            public Func<dynamic, string> SerializeFunc { get; set; }

            public string SerializeData(dynamic data) => SerializeFunc(data);
        }

        [Fact]
        public void StrategyWithFuncs()
        {
            var data = new User { Id = 2, Name = "Test user" };

            var help = new SerializerDictHelper();
            var xml = help.SerializeData(SerializerType.Xml, data);

            var help2 = new SerializeHelper();
            help2.SerializeFunc = (s) => s.ToString();
            var result = help2.SerializeData(data);
        }
    }
}