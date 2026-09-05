using FacilityQuote.Domain.Services;

namespace FacilityQuote.Application.Services;

public interface IServiceRepository
{
    Task<Service?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Service>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Service>> GetActiveAsync(CancellationToken cancellationToken = default);

    Task AddAsync(Service service, CancellationToken cancellationToken = default);

    Task UpdateAsync(Service service, CancellationToken cancellationToken = default);
}