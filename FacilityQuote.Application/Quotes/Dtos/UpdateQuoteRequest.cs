namespace FacilityQuote.Application.Quotes.Dtos;

public record UpdateQuoteRequest(
    DateOnly? ValidUntil,
    string? Notes,
    decimal TravelCost);