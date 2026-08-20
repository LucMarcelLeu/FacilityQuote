using FacilityQuote.Application.Services;
using FacilityQuote.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace FacilityQuote.Infrastructure.Persistence;

public class ServiceRepository : IServiceRepository
{
    private readonly FacilityQuoteDbContext _context;

    public ServiceRepository(FacilityQuoteDbContext context)
    {
        _context = context;
    }

    public async Task<Service?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Services
            .SingleOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task<IReadOnlyList<Service>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Services
            .Where(x => x.IsActive)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        Service service,
        CancellationToken cancellationToken = default)
    {
        await _context.Services.AddAsync(service, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }
}