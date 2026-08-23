using FacilityQuote.Domain.Availability;
using FacilityQuote.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FacilityQuote.Infrastructure.Repositories;

public class AvailabilityRepository : IAvailabilityRepository
{
    private readonly FacilityQuoteDbContext _context;

    public AvailabilityRepository(FacilityQuoteDbContext context)
    {
        _context = context;
    }

    public async Task<AvailabilitySlot?> GetByDateAsync(
        DateOnly date,
        CancellationToken cancellationToken = default)
    {
        return await _context.Availabilities
            .FirstOrDefaultAsync(
                x => x.Date == date,
                cancellationToken);
    }

    public async Task<IReadOnlyList<AvailabilitySlot>> GetRangeAsync(
        DateOnly from,
        DateOnly to,
        CancellationToken cancellationToken = default)
    {
        return await _context.Availabilities
            .Where(x => x.Date >= from && x.Date <= to)
            .OrderBy(x => x.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        AvailabilitySlot availability,
        CancellationToken cancellationToken = default)
    {
        await _context.Availabilities.AddAsync(
            availability,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(
    CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}