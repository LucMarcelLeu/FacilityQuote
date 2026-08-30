using FacilityQuote.Api.Models.Services;
using FacilityQuote.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FacilityQuote.Api.Controllers;

[ApiController]
[Route("api/services")]
public class ServicesController : ControllerBase
{
    private readonly ServicesService _serviceService;

    public ServicesController(ServicesService serviceService)
    {
        _serviceService = serviceService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateServiceRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateServiceCommand(
            request.ServiceCategory,
            request.Name,
            request.IsActive,
            request.Description);

        var createdRequest = await _serviceService.CreateAsync(
            command,
            cancellationToken);

        return Ok(new
        {
            createdRequest.Id
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
    CancellationToken cancellationToken)
    {
        var services = await _serviceService.GetAllAsync(
            cancellationToken);

        return Ok(services.Select(x => new
        {
            x.Id,
            x.Category,
            x.Name,
            x.Description,
            x.Unit
        }));
    }
}