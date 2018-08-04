using Xunit;

namespace PatternsAndPrinciples.Patterns.GoF.Creational
{
    /*
     * Allows a client to create families of objects without specifying their concrete classes
     * 
     * Different MessageQueue implementations that are interchangable
     * MessageQueue factories create specific Outbound and Reply queues
     */

    public interface IMessageQueue { }

    public class MsmqMessageQueue : IMessageQueue { }

    public class MsmqResponseMessageQueue : IMessageQueue { }

    public class AzureMessageQueue : IMessageQueue { }

    public class AzureResponseMessageQueue : IMessageQueue { }

    public interface IMessageQueueFactory
    {
        IMessageQueue CreateOutboundQueue(string name);

        IMessageQueue CreateReplyQueue(string name);
    }

    public class AzureServiceBusQueueFactory : IMessageQueueFactory
    {
        public IMessageQueue CreateOutboundQueue(string name)
        {
            return new AzureMessageQueue(/*....*/);
        }

        public IMessageQueue CreateReplyQueue(string name)
        {
            return new AzureResponseMessageQueue(/*....*/);
        }
    }

    public class MsmqFactory : IMessageQueueFactory
    {
        public IMessageQueue CreateOutboundQueue(string name)
        {
            return new MsmqMessageQueue(/*....*/);
        }

        public IMessageQueue CreateReplyQueue(string name)
        {
            return new MsmqResponseMessageQueue(/*....*/);
        }
    }

    public class Producer
    {
        private readonly IMessageQueueFactory _queueFactory;

        public Producer(IMessageQueueFactory queueFactory) => _queueFactory = queueFactory;

        public void SendData()
        {
            var queue = _queueFactory.CreateOutboundQueue("aabbaa22");

            // Get latest data
            var data = new { payload = 23.0 };
            //queue.SendData(data);

            // Possibly dispose queue etc.
        }
    }

    public class AbstractFactoryTest
    {
        [Fact]
        public void Test()
        {
            // Get correct factory type from configuration
            var factoryType = "msmq";

            var factory = factoryType == "azure"
                                    ? new AzureServiceBusQueueFactory() as IMessageQueueFactory
                                    : new MsmqFactory();

            var producer = new Producer(factory);
            producer.SendData();
        }
    }
}