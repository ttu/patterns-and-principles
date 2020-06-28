using System;
using System.Collections.Generic;
using Xunit;

namespace PatternsAndPrinciples.UML
{
    /*
     * An association with an aggregation relationship indicates that one class is a part of another class.
     * In an aggregation relationship, the child class instance can outlive its parent class.
     *
     * All objects have their own lifecycle but there is an ownership:
     * a child object can not belong to another parent object.
     *
     * The composition aggregation relationship is just another form of the aggregation relationship,
     * but the child class's instance lifecycle is dependent on the parent class's instance lifecycle
     */

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

    public class Department
    {
        public Department(string id) => Id = id;

        public string Id { get; }
    }

    public class Company
    {
        private List<Department> _departments = new List<Department>();

        public void AddDepartment(string id) => _departments.Add(new Department(id));

        public void HandleDepartments()
        {
            /* ... */
        }
    }

    public class CompositionAggregationTests
    {
        [Fact]
        public void Tests()
        {
            var company = new Company();
            company.AddDepartment("A1");
            company.AddDepartment("A2");

            // Departments are also destroyend
            company = null;
        }
    }
}