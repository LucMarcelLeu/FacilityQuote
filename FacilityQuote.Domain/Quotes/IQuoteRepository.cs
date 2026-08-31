using FacilityQuote.Domain.Quotes;
using FacilityQuote.Domain.Requests;

namespace FacilityQuote.Application.Quotes;

public interface IQuoteRepository
{
    Task<Quote?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Quote>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Quote?> GetByRequestIdAsync(Guid requestId, CancellationToken cancellationToken = default);

    Task<int> GetNextQuoteNumberAsync(CancellationToken cancellationToken = default);

    Task AddAsync(Quote quote, CancellationToken cancellationToken = default);

    Task AddItemAsync(QuoteItem item, CancellationToken cancellationToken = default);

    void RemoveItem(QuoteItem item);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<(Quote Quote, Request Request)?> GetForPdfAsync(Guid id, CancellationToken cancellationToken = default);
}