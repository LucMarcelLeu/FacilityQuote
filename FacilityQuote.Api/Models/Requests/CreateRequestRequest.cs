namespace FacilityQuote.Api.Models.Requests;

public sealed record CreateRequestRequest(
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

    DateOnly DesiredDate,
    TimeOnly EarliestTime,
    TimeOnly LatestTime,

    string? Description);