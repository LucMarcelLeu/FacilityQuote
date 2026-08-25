namespace FacilityQuote.Application.Customers;

public record CustomerDto(
    Guid Id,
    string FirstName,
    string LastName,
    string? CompanyName,
    string Street,
    string PostalCode,
    string City,
    string Email,
    string? Phone);