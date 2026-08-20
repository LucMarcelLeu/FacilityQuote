using FacilityQuote.Domain.Locations;

namespace FacilityQuote.Domain.Customers;

public class Customer
{
    public Guid Id { get; private set; }

    public string FirstName { get; private set; } = string.Empty;

    public string LastName { get; private set; } = string.Empty;

    public string? CompanyName { get; private set; }

    public Address Address { get; private set; } = null!;

    public string Email { get; private set; } = string.Empty;

    public string? Phone { get; private set; }

    private Customer()
    {
    }

    public Customer(
        string firstName,
        string lastName,
        string? companyName,
        Address address,
        string email,
        string? phone)
    {
        Id = Guid.NewGuid();

        FirstName = firstName;
        LastName = lastName;
        CompanyName = companyName;
        Address = address;
        Email = email;
        Phone = phone;
    }
}