namespace FacilityQuote.Api.Models.Availability;

public sealed record AvailabilityResponse(
    DateOnly Date,
    bool MorningAvailable,
    bool AfternoonAvailable);