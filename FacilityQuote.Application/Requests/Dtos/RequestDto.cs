namespace FacilityQuote.Application.Requests.Dtos;

public class RequestDto
{
    public Guid Id { get; init; }

    public Guid CustomerId { get; init; }

    public string CustomerName { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public string? Phone { get; init; }

    public Guid ServiceId { get; init; }

    public string Service { get; init; } = string.Empty;

    public DateOnly DesiredDate { get; init; }

    public TimeOnly EarliestTime { get; init; }

    public TimeOnly LatestTime { get; init; }

    public string Street { get; init; } = string.Empty;

    public string PostalCode { get; init; } = string.Empty;

    public string City { get; init; } = string.Empty;

    public string? Description { get; init; }

    public string Status { get; init; } = string.Empty;

    public DateTime CreatedAt { get; init; }
}