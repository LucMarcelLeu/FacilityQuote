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
            command.Unit,
            command.UnitPrice,
            command.Description);

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

    public async Task<IReadOnlyList<Service>> GetActiveAsync(
        CancellationToken cancellationToken = default)
    {
        return await _serviceRepository.GetActiveAsync(
            cancellationToken);
    }

    public async Task<Service> UpdateAsync(
        Guid id,
        UpdateServiceCommand command,
        CancellationToken cancellationToken = default)
    {
        var service = await _serviceRepository.GetByIdAsync(
            id,
            cancellationToken);

        if (service is null)
            throw new KeyNotFoundException(
                $"Service with id '{id}' was not found.");

        service.Update(
            command.ServiceCategory,
            command.Name,
            command.IsActive,
            command.Description,
            command.Unit,
            command.UnitPrice);

        await _serviceRepository.UpdateAsync(
            service,
            cancellationToken);

        return service;
    }

    public async Task<Service> ActivateAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var service = await _serviceRepository.GetByIdAsync(
            id,
            cancellationToken);

        if (service is null)
            throw new KeyNotFoundException(
                $"Service with id '{id}' was not found.");

        service.Activate();

        await _serviceRepository.UpdateAsync(
            service,
            cancellationToken);

        return service;
    }

    public async Task<Service> DeactivateAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var service = await _serviceRepository.GetByIdAsync(
            id,
            cancellationToken);

        if (service is null)
            throw new KeyNotFoundException(
                $"Service with id '{id}' was not found.");

        service.Deactivate();

        await _serviceRepository.UpdateAsync(
            service,
            cancellationToken);

        return service;
    }
}