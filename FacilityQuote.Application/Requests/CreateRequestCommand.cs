namespace FacilityQuote.Application.Requests;

public sealed record CreateRequestCommand(
    string FirstName,
    string LastName,
    string? CompanyName,

    string CustomerStreet,
    string CustomerPostalCode,
    string CustomerCity,

    string Email,
    string? Phone,

    string LocationStreet,
    string LocationPostalCode,
    string LocationCity,

    Guid ServiceId,
    decimal? Quantity,

    DateOnly DesiredDate,
    TimeOnly EarliestTime,
    TimeOnly LatestTime,

    string? Description);