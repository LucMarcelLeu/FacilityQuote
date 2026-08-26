using FacilityQuote.Application.Customers.Dtos;
using FacilityQuote.Domain.Customers;

namespace FacilityQuote.Application.Customers;

public class CustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<IReadOnlyList<CustomerDto>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var customers = await _customerRepository.GetAllAsync(
            cancellationToken);

        return customers
            .Select(ToDto)
            .ToList();
    }

    public async Task<Customer?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        return await _customerRepository.GetByEmailAsync(
            email,
            cancellationToken);
    }

    public async Task<Customer?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _customerRepository.GetByIdAsync(
            id,
            cancellationToken);
    }

    private static CustomerDto ToDto(Customer customer)
    {
        return new CustomerDto
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            CompanyName = customer.CompanyName,

            Street = customer.Address.Street,
            PostalCode = customer.Address.PostalCode,
            City = customer.Address.City,

            Email = customer.Email,
            Phone = customer.Phone,

            Requests = customer.Requests
                .Select(request => new Api.Models.Customers.CustomerRequestDto
                {
                    Id = request.Id,

                    Service = request.Service.Name,

                    DesiredDate = request.DesiredDate,

                    EarliestTime = request.EarliestTime,
                    LatestTime = request.LatestTime,

                    Status = request.Status.ToString(),

                    Street = request.Location.Street,
                    PostalCode = request.Location.PostalCode,
                    City = request.Location.City
                })
                .ToList()
        };
    }
}