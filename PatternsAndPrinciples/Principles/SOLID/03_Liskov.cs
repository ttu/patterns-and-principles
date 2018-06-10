using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PatternsAndPinciples.Principles.SOLID
{
    public abstract class MessageBusConnection
    {
        public abstract void Send(string topic, object payload);
    }

    public class RabbitMQConnection : MessageBusConnection
    {
        public override void Send(string topic, object payload)
        {
            // Send to RabbitMQ
        }
    }

    public class ExtendedConnection : RabbitMQConnection
    {
        // Because ExtendedConnection inherits RabbitMQConnection, it needs to send payload to RabbitMQ
        public override void Send(string topic, object payload)
        {
            base.Send(topic, payload);
            // Send to somewhere else
        }
    }

    public class MessageProcessor
    {
        private readonly string _topic = "Worker_Queue_1";
        private readonly RabbitMQConnection _bus;

        public MessageProcessor(RabbitMQConnection bus) => _bus = bus;

        public void ProcessMessages(List<dynamic> datas)
        {
            foreach (var data in datas)
            {
                _bus.Send(_topic, data);
            }
        }
    }

    public class LiskovSubstitionPrincipleTest
    {
        [Fact]
        public void Test()
        {
            // MessageProcessor thinks it is only sending to RabbitMQ but in reality it is also doing something else
            var processor = new MessageProcessor(new ExtendedConnection());
            processor.ProcessMessages(new List<dynamic> { "ok", 1 });
        }
    }
}
