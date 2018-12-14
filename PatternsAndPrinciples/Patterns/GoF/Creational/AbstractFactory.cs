using Xunit;

namespace PatternsAndPrinciples.Patterns.GoF.Creational
{
    /*
     * Allows a client to create families of objects without specifying their concrete classes
     * 
     * MessageQueue factories create specific Outbound and Response queues
     * MessageQueue implementations that are interchangable
     */

    public interface IMessageQueue { }

    public class AwsMessageQueue : IMessageQueue { }

    public class AwsResponseMessageQueue : IMessageQueue { }

    public class AzureMessageQueue : IMessageQueue { }

    public class AzureResponseMessageQueue : IMessageQueue { }

    public interface IMessageQueueFactory
    {
        IMessageQueue CreateOutboundQueue(string name);

        IMessageQueue CreateResponseQueue(string name);
    }

    public class AzureServiceBusQueueFactory : IMessageQueueFactory
    {
        public IMessageQueue CreateOutboundQueue(string name)
        {
            return new AzureMessageQueue(/*....*/);
        }

        public IMessageQueue CreateResponseQueue(string name)
        {
            return new AzureResponseMessageQueue(/*....*/);
        }
    }

    public class AwsFactory : IMessageQueueFactory
    {
        public IMessageQueue CreateOutboundQueue(string name)
        {
            return new AwsMessageQueue(/*....*/);
        }

        public IMessageQueue CreateResponseQueue(string name)
        {
            return new AwsResponseMessageQueue(/*....*/);
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
            var factoryType = "aws";

            var factory = factoryType == "azure"
                                    ? new AzureServiceBusQueueFactory() as IMessageQueueFactory
                                    : new AwsFactory();

            var producer = new Producer(factory);
            producer.SendData();
        }
    }
}