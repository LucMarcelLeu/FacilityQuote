namespace FacilityQuote.Api.Models.Customers;

public class CustomerRequestDto
{
    public Guid Id { get; init; }

    public string Service { get; init; } = string.Empty;

    public DateOnly DesiredDate { get; init; }

    public TimeOnly EarliestTime { get; init; }

    public TimeOnly LatestTime { get; init; }

    public string Status { get; init; } = string.Empty;

    public string Street { get; init; } = string.Empty;

    public string PostalCode { get; init; } = string.Empty;

    public string City { get; init; } = string.Empty;
}