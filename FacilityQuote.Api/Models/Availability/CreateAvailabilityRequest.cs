namespace FacilityQuote.Api.Models.Availability;

public sealed record CreateAvailabilityRequest(
    DateOnly Date,
    bool MorningAvailable,
    bool AfternoonAvailable);

    