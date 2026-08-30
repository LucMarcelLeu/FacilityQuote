namespace FacilityQuote.Domain.Quotes;

public class Quote
{
    public Guid Id { get; set; }

    public Guid RequestId { get; set; }

    public string QuoteNumber { get; set; } = string.Empty;

    public QuoteStatus Status { get; set; } = QuoteStatus.Draft;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ValidUntil { get; set; }

    public string? Notes { get; set; }

    public decimal TravelCost { get; set; }

    public ICollection<QuoteItem> Items { get; set; } = new List<QuoteItem>();

    public decimal Subtotal =>
        Items.Sum(item => item.Total);

    public decimal Total =>
        Subtotal + TravelCost;
}