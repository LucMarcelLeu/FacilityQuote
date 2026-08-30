using FacilityQuote.Application.Quotes;
using FacilityQuote.Domain.Quotes;
using Microsoft.EntityFrameworkCore;

namespace FacilityQuote.Infrastructure.Persistence;

public class QuoteRepository(
    FacilityQuoteDbContext context) : IQuoteRepository
{
    public async Task<Quote?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await context.Quotes
            .Include(q => q.Items)
            .FirstOrDefaultAsync(
                q => q.Id == id,
                cancellationToken);
    }

    public async Task<IReadOnlyList<Quote>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await context.Quotes
            .Include(q => q.Items)
            .OrderByDescending(q => q.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Quote?> GetByRequestIdAsync(
        Guid requestId,
        CancellationToken cancellationToken = default)
    {
        return await context.Quotes
            .Include(q => q.Items)
            .FirstOrDefaultAsync(
                q => q.RequestId == requestId,
                cancellationToken);
    }

    public async Task<int> GetNextQuoteNumberAsync(
    CancellationToken cancellationToken = default)
    {
        var result = await context.Database
            .SqlQueryRaw<int>(
                """SELECT nextval('quote_number_seq') AS "Value" """)
            .SingleAsync(cancellationToken);

        return result;
    }

    public async Task AddAsync(
        Quote quote,
        CancellationToken cancellationToken = default)
    {
        await context.Quotes.AddAsync(
            quote,
            cancellationToken);
    }
    public async Task AddItemAsync(
        QuoteItem item,
        CancellationToken cancellationToken = default)
    {
        await context.QuoteItems.AddAsync(
            item,
            cancellationToken);
    }

    public void RemoveItem(QuoteItem item)
    {
        context.QuoteItems.Remove(item);
    }
    
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}