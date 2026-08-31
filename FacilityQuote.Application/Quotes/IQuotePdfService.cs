namespace FacilityQuote.Application.Quotes;

public interface IQuotePdfService
{
    Task<QuotePdfResult> GenerateAsync(Guid quoteId, CancellationToken cancellationToken = default);
}

public sealed record QuotePdfResult(byte[] Content, string FileName);