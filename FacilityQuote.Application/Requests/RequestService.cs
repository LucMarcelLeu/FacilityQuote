using FacilityQuote.Application.Customers;
using FacilityQuote.Application.Services;
using FacilityQuote.Domain.Customers;
using FacilityQuote.Domain.Locations;
using FacilityQuote.Domain.Requests;

namespace FacilityQuote.Application.Requests;

public class RequestService
{
    private readonly IRequestRepository _requestRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly ICustomerRepository _customerRepository;

    private readonly IAvailabilityRepository _availabilityRepository;

    public RequestService(
        IRequestRepository requestRepository,
        IServiceRepository serviceRepository,
        ICustomerRepository customerRepository,
        IAvailabilityRepository availabilityRepository)
    {
        _requestRepository = requestRepository;
        _serviceRepository = serviceRepository;
        _customerRepository = customerRepository;
        _availabilityRepository = availabilityRepository;
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

        var availability = await _availabilityRepository.GetByDateAsync(
            command.DesiredDate,
            cancellationToken);

        if (availability is null)
        {
            throw new InvalidOperationException(
                $"No availability exists for {command.DesiredDate}.");
        }

        if (!availability.IsAvailable(command.TimeSlot))
        {
            throw new InvalidOperationException(
                $"The {command.TimeSlot} slot is not available.");
        }


        if (!service.IsActive)
        {
            throw new InvalidOperationException(
                $"Service '{command.ServiceId}' is not active.");
        }

        var customer = await _customerRepository.GetByEmailAsync(
            command.Email,
            cancellationToken);

        if (customer is null)
        {
            var customerAddress = new Address(
                command.CustomerStreet,
                command.CustomerPostalCode,
                command.CustomerCity);

            customer = new Customer(
                command.FirstName,
                command.LastName,
                command.CompanyName,
                customerAddress,
                command.Email,
                command.Phone);

            await _customerRepository.AddAsync(
                customer,
                cancellationToken);
        }

        /*
         * Standort der Anfrage.
         */
        var location = new Address(
            command.LocationStreet,
            command.LocationPostalCode,
            command.LocationCity);

        /*
         * Anfrage erstellen.
         */
        var request = new Request(
            customer,
            service,
            availability,
            command.TimeSlot,
            command.DesiredDate,
            command.EarliestTime,
            command.LatestTime,
            location,
            command.Description,
            command.Quantity);

        await _requestRepository.AddAsync(
            request,
            cancellationToken);

        return request;
    }

    public async Task<Request?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _requestRepository.GetByIdAsync(
            id,
            cancellationToken);
    }

    public async Task<Request?> UpdateStatusAsync(
    Guid id,
    RequestStatus status,
    CancellationToken cancellationToken = default)
    {
        var request = await _requestRepository.GetByIdAsync(
            id,
            cancellationToken);

        if (request is null)
        {
            return null;
        }

        switch (status)
        {
            case RequestStatus.Reviewing:
                request.StartReview();
                break;

            case RequestStatus.QuotationCreated:
                request.MarkQuotationCreated();
                break;

            case RequestStatus.Rejected:
                request.Reject();
                break;

            default:
                throw new ArgumentException(
                    $"Status '{status}' cannot be set manually.");
        }

        await _requestRepository.UpdateAsync(
            request,
            cancellationToken);

        return request;
    }

}