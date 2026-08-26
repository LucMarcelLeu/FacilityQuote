using FacilityQuote.Domain.Requests;

namespace FacilityQuote.Application.Requests;

public interface IRequestRepository
{
    Task AddAsync(Request request, CancellationToken cancellationToken = default);

    Task<Request?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task UpdateAsync(Request request, CancellationToken cancellationToken = default);
}