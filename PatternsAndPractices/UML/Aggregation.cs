using System;
using Xunit;

namespace PatternsAndPractices.UML
{
    public class Invoice
    {
        public Invoice(Customer customer) => Customer = customer;

        public Guid InvoiceId { get; } = Guid.NewGuid();
        public Customer Customer { get; }
    }

    public class Customer
    {
        public int CustomerId { get; set; }
    }

    public class AggregationTests
    {
        [Fact]
        public void Tests()
        {
            var customer = new Customer { CustomerId = 3 };
            var invoice = new Invoice(customer);

            invoice = null;
        }
    }
}