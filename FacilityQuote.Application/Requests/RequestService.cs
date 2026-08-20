using FacilityQuote.Application.Services;
using FacilityQuote.Domain.Customers;
using FacilityQuote.Domain.Locations;
using FacilityQuote.Domain.Requests;

namespace FacilityQuote.Application.Requests;

public class RequestService
{
    private readonly IRequestRepository _requestRepository;
    private readonly IServiceRepository _serviceRepository;

    public RequestService(
        IRequestRepository requestRepository,
        IServiceRepository serviceRepository)
    {
        _requestRepository = requestRepository;
        _serviceRepository = serviceRepository;
    }

    public async Task<Request> CreateAsync(
        CreateRequestCommand command,
        CancellationToken cancellationToken = default)
    {
        var service = await _serviceRepository.GetByIdAsync(
            command.ServiceId,
            cancellationToken);

        if (service is null)
        {
            throw new InvalidOperationException(
                $"Service '{command.ServiceId}' was not found.");
        }

        if (!service.IsActive)
        {
            throw new InvalidOperationException(
                $"Service '{command.ServiceId}' is not active.");
        }

        var customerAddress = new Address(
            command.CustomerStreet,
            command.CustomerPostalCode,
            command.CustomerCity);

        var customer = new Customer(
            command.FirstName,
            command.LastName,
            command.CompanyName,
            customerAddress,
            command.Email,
            command.Phone);

        var location = new Address(
            command.LocationStreet,
            command.LocationPostalCode,
            command.LocationCity);

        var request = new Request(
            customer,
            service,
            command.DesiredDate,
            command.EarliestTime,
            command.LatestTime,
            location,
            command.Description);

        await _requestRepository.AddAsync(
            request,
            cancellationToken);

        return request;
    }
}