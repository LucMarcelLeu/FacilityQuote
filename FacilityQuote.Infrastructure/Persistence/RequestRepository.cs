using FacilityQuote.Application.Requests;
using FacilityQuote.Domain.Requests;

namespace FacilityQuote.Infrastructure.Persistence;

public class RequestRepository : IRequestRepository
{
    private readonly FacilityQuoteDbContext _context;

    public RequestRepository(FacilityQuoteDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        Request request,
        CancellationToken cancellationToken = default)
    {
        await _context.Requests.AddAsync(request, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }
}