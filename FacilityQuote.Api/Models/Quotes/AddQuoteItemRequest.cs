namespace FacilityQuote.Api.Models.Quotes;

public record AddQuoteItemRequest(
    string Description,
    decimal Quantity,
    string Unit,
    decimal UnitPrice);