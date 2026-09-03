using FacilityQuote.Application.Requests;
using FacilityQuote.Domain.Requests;
using Microsoft.EntityFrameworkCore;

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
        Console.WriteLine($"Quantity foo: {request.Quantity}");

        await _context.Requests.AddAsync(request, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Request?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Requests
            .Include(r => r.Customer)
            .Include(r => r.Service)
            .FirstOrDefaultAsync(
                r => r.Id == id,
                cancellationToken);
    }

    public async Task UpdateAsync(
        Request request,
        CancellationToken cancellationToken = default)
    {
        _context.Requests.Update(request);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Request>> GetByDateRangeAsync(
        DateOnly from,
        DateOnly to,
        CancellationToken cancellationToken = default)
    {
        return await _context.Requests
            .Where(r => r.DesiredDate >= from && r.DesiredDate <= to)
            .ToListAsync(cancellationToken);
    }

}