namespace FacilityQuote.Application.Customers.Dtos;

public class CustomerDto
{
    public Guid Id { get; init; }

    public string FirstName { get; init; } = string.Empty;

    public string LastName { get; init; } = string.Empty;

    public string? CompanyName { get; init; }

    public string Street { get; init; } = string.Empty;

    public string PostalCode { get; init; } = string.Empty;

    public string City { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public string? Phone { get; init; }

    public List<Api.Models.Customers.CustomerRequestDto> Requests { get; init; } = [];
}