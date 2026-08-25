using FacilityQuote.Domain.Customers;

namespace FacilityQuote.Application.Customers;

public class CustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<IReadOnlyList<CustomerDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var customers = await _customerRepository.GetAllAsync(
            cancellationToken);

        return customers
            .Select(ToDto)
            .ToList();
    }


    public async Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _customerRepository.GetByEmailAsync(
            email,
            cancellationToken);
    }

    public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _customerRepository.GetByIdAsync(
            id,
            cancellationToken);
    }

    private static CustomerDto ToDto(Customer customer)
    {
        return new CustomerDto(
            customer.Id,
            customer.FirstName,
            customer.LastName,
            customer.CompanyName,
            customer.Address.Street,
            customer.Address.PostalCode,
            customer.Address.City,
            customer.Email,
            customer.Phone);
    }
}