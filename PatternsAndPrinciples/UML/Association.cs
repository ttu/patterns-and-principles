using System;
using System.Collections.Generic;
using Xunit;

namespace PatternsAndPinciples.UML
{
    public class Flight
    {
        public string Id { get; set; }
        public string Destination { get; set; }
        public Plane AssginedPlane { get; set; }
    }

    public class Plane
    {
        public string Id { get; set; }
        public string Model { get; set; }
        public List<Flight> Flights { get; set; } = new List<Flight>();
    }

    public class FlightManifest
    {
        public Flight Flight { get; set; }
        public List<Passenger> Passenger { get; set; } = new List<Passenger>();
    }

    public class Passenger
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class AssociationTests
    {
        [Fact]
        public void Bi_Directional_Tests()
        {
            var FKH1Ber = new Flight { Id = "FKH1Ber", Destination = "Berlin" };
            var FKH3NY = new Flight { Id = "FKH3NY", Destination = "New York" };

            var plane1 = new Plane { Id = "AB2", Model = "Airbus 222" };

            FKH1Ber.AssginedPlane = plane1;
            FKH3NY.AssginedPlane = plane1;

            plane1.Flights.Add(FKH1Ber);
            plane1.Flights.Add(FKH3NY);
        }

        [Fact]
        public void Uni_Directional_Tests()
        {
            var FKH1Ber = new Flight { Id = "FKH1Ber", Destination = "Berlin" };

            var manifest = new FlightManifest { Flight = FKH1Ber };

            var passenger1 = new Passenger { Id = "AA", Name = "James" };
            var passenger2 = new Passenger { Id = "AB", Name = "Mary" };

            manifest.Passenger.Add(passenger1);
            manifest.Passenger.Add(passenger2);
        }
    }
}