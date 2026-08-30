namespace FacilityQuote.Application.Quotes.Dtos;

public record UpdateQuoteRequest(
    DateTime? ValidUntil,
    string? Notes,
    decimal TravelCost,
    IReadOnlyList<UpdateQuoteItemRequest> Items);