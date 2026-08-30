namespace FacilityQuote.Domain.Quotes;

public class QuoteItem
{
    public Guid Id { get; set; }

    public Guid QuoteId { get; set; }

    public Quote Quote { get; set; } = null!;

    public string Description { get; set; } = string.Empty;

    public decimal Quantity { get; set; }

    public string Unit { get; set; } = string.Empty;

    public decimal UnitPrice { get; set; }

    public decimal Total => Quantity * UnitPrice;
}