namespace FacilityQuote.Domain.Locations;

public class Address
{
    public string Street { get; private set; } = string.Empty;

    public string PostalCode { get; private set; } = string.Empty;

    public string City { get; private set; } = string.Empty;

    private Address()
    {
    }

    public Address(
        string street,
        string postalCode,
        string city)
    {
        Street = street;
        PostalCode = postalCode;
        City = city;
    }
}