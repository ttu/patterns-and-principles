using Newtonsoft.Json;
using System.IO;
using System.Xml.Serialization;
using Xunit;

namespace PatternsAndPinciples.Patterns.GoF.Behavioral
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

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, data);
                return textWriter.ToString();
            }
        }
    }

    public class DataParser
    {
        public Strategy Serializer { get; set; }

        public string SerializerData<T>(T data)
        {
            return Serializer.Serialize(data);
        }
    }

    public class StrategyTests
    {
        [Fact]
        public void CommandsWithFunctions()
        {
            var parser = new DataParser();

            var xmlStrategy = new XmlStrategy();
            var jsonStrategy = new JsonStrategy();

            parser.Serializer = jsonStrategy;

            var data = new User { Id = 2, Name = "Test user" };

            var json = parser.SerializerData(data);

            parser.Serializer = xmlStrategy;

            var xml = parser.SerializerData(data);
        }

        public class User
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}