using Xunit;

namespace PatternsAndPrinciples.Patterns.GoF.Creational
{
    /*
     * Creates objects without exposing the instantiation logic to the client
     */

    public class RabbitMqResponseQueue
    {
        private readonly string _address;

        public RabbitMqResponseQueue(string address) => _address = address;

        public dynamic GetData() => true;
    }

    public class RabbitMqRequestQueue
    {
        private readonly string _address;

        public RabbitMqRequestQueue(string address) => _address = address;

        public bool SendData(dynamic data) => true;
    }

    public class RabbitMqMessageQueueFactory
    {
        private readonly string _address;

        public RabbitMqMessageQueueFactory(string address) => _address = address;

        public RabbitMqResponseQueue CreateResponseQueue() => new RabbitMqResponseQueue(_address);

        public RabbitMqRequestQueue CreateRequestQueue() => new RabbitMqRequestQueue(_address);
    }

    public class RabbitMqProducer
    {
        private readonly RabbitMqMessageQueueFactory _queueFactory;

        public RabbitMqProducer(RabbitMqMessageQueueFactory queueFactory) => _queueFactory = queueFactory;

        public void SendData()
        {
            var queue = _queueFactory.CreateRequestQueue();

            // Send latest data
            var data = new { payload = 23.0 };
            queue.SendData(data);

            // Possibly dispose queue etc.
        }
    }

    public class FactoryTest
    {
        [Fact]
        public void Test()
        {
            var factory = new RabbitMqMessageQueueFactory("localhost");

            var sender = new RabbitMqProducer(factory);
            sender.SendData();
        }
    }
}