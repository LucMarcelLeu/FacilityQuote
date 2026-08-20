using FacilityQuote.Domain.Requests;

namespace FacilityQuote.Application.Requests;

public interface IRequestRepository
{
    Task AddAsync(Request request, CancellationToken cancellationToken = default);
}