using FacilityQuote.Domain.Services;

namespace FacilityQuote.Application.Services;

public class ServicesService
{
    private readonly IServiceRepository _serviceRepository;

    public ServicesService(
        IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public async Task<Service> CreateAsync(
        CreateServiceCommand command,
        CancellationToken cancellationToken = default)
    {

        var service = new Service(
            command.ServiceCategory,
            command.Name,
            command.IsActive,
            command.Description
            );

        await _serviceRepository.AddAsync(
            service,
            cancellationToken);

        return service;
    }

    public async Task<IReadOnlyList<Service>> GetAllAsync(
    CancellationToken cancellationToken = default)
    {
        return await _serviceRepository.GetAllAsync(cancellationToken);
    }
}