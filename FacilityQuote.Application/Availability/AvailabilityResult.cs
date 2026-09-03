namespace FacilityQuote.Application.Availability;

public record AvailabilityResult(
    DateOnly Date,
    bool MorningAvailable,
    bool AfternoonAvailable,
    bool MorningBooked,
    bool AfternoonBooked);