using FacilityQuote.Api.Models.Customers;
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
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
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

        var dto = new CustomerDto
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            CompanyName = customer.CompanyName,
            Email = customer.Email,
            Phone = customer.Phone,

            Street = customer.Address.Street,
            PostalCode = customer.Address.PostalCode,
            City = customer.Address.City,

            Requests = customer.Requests
                .Select(request => new CustomerRequestDto
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

        return Ok(dto);
    }
}