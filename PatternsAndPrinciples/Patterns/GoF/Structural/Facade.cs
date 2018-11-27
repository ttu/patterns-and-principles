using System.Collections.Generic;
using Xunit;

namespace PatternsAndPinciples.Patterns.GoF.Structural
{
    /*
     * Simpifies the interface of a set of classes
     *
     * A single class that represents an entire subsystem
     */

    public class Order
    {
        public List<object> Items { get; set; }
    }

    public class OrderFacade
    {
        private readonly WarehouseService _warehouse;
        private readonly BillingService _billing;

        public OrderFacade(WarehouseService warehouse, BillingService billing)
        {
            _warehouse = warehouse;
            _billing = billing;
        }

        public (bool success, string url) HandleOrder(Order order)
        {
            var reserveOk = _warehouse.ReserveItems(order.Items);

            if (reserveOk == false)
                return (false, string.Empty);

            var payment = _billing.CreatePayment(order);

            if (!payment.ok)
                _warehouse.ReleaseItems(order.Items);

            return payment;
        }
    }

    public class WarehouseService
    {
        public bool ReserveItems(dynamic items)
        {
            // Remove order items from storage
            return true;
        }

        internal void ReleaseItems(dynamic items)
        {
            // Add order items back to storage
        }
    }

    public class BillingService
    {
        internal (bool ok, string url) CreatePayment(dynamic order)
        {
            return (true, "http://payment.url");
        }
    }

    public class FacadeTests
    {
        [Fact]
        public void Test()
        {
            var warehouse = new WarehouseService();
            var billiing = new BillingService();

            var orderFacade = new OrderFacade(warehouse, billiing);

            // e.g. OrderFacade is injected to Web API Controller

            var order = new Order { Items = new List<object>() };
            var result = orderFacade.HandleOrder(order);
        }
    }
}