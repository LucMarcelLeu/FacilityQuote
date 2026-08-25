using FacilityQuote.Application.Customers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FacilityQuote.Api.Controllers;

[ApiController]
[Route("api/admin/customers")]
[Authorize(Roles = "admin")]
public class CustomersController : ControllerBase
{
    private readonly CustomerService _customerService;

    public CustomersController(CustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var customers = await _customerService.GetAllAsync(
            cancellationToken);

        return Ok(customers);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var customer = await _customerService.GetByIdAsync(
            id,
            cancellationToken);

        if (customer is null)
        {
            return NotFound();
        }

        return Ok(new
        {
            customer.Id,
            customer.FirstName,
            customer.LastName,
            customer.CompanyName,
            customer.Email,
            customer.Phone,
            Street = customer.Address.Street,
            PostalCode = customer.Address.PostalCode,
            City = customer.Address.City
        });
    }
}