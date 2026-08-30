namespace FacilityQuote.Application.Quotes.Dtos;

public record UpdateQuoteItemRequest(
    string Description,
    decimal Quantity,
    string Unit,
    decimal UnitPrice);
