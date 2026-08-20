using FacilityQuote.Domain.Customers;

namespace FacilityQuote.Tests.Customers;

public class CustomerTests
{
    [Fact]
    public void Constructor_ShouldCreateCustomer()
    {
        // Arrange & Act
        var customer = new Customer(
            "Max",
            "Muster",
            "Muster AG",
            new Domain.Locations.Address("Bahnhofstrasse 10", "8001","Zürich"),
            "max@muster.ch",
            "+41 79 123 45 67");

        // Assert
        Assert.NotEqual(Guid.Empty, customer.Id);

        Assert.Equal("Max", customer.FirstName);
        Assert.Equal("Muster", customer.LastName);
        Assert.Equal("Muster AG", customer.CompanyName);

        Assert.Equal("Bahnhofstrasse 10", customer.Address.Street);
        Assert.Equal("8001", customer.Address.PostalCode);
        Assert.Equal("Zürich", customer.Address.City);

        Assert.Equal("max@muster.ch", customer.Email);
        Assert.Equal("+41 79 123 45 67", customer.Phone);
    }

    [Fact]
    public void Constructor_ShouldAllowPrivateCustomerWithoutCompany()
    {
        // Arrange & Act
        var customer = new Customer(
            "Anna",
            "Muster",
            null,
            new Domain.Locations.Address("Dorfstrasse 5", "8000", "Zürich"),
            "anna@example.com",
            null);

        // Assert
        Assert.Null(customer.CompanyName);
        Assert.Null(customer.Phone);

        Assert.Equal("Anna", customer.FirstName);
        Assert.Equal("Muster", customer.LastName);
    }
}