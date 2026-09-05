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
            request.Description,
            request.Unit,
            request.UnitPrice);

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
            x.Unit,
            x.UnitPrice,
            x.IsActive
        }));
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActive(
        CancellationToken cancellationToken)
    {
        var services = await _serviceService.GetActiveAsync(
            cancellationToken);

        return Ok(services.Select(x => new
        {
            x.Id,
            x.Category,
            x.Name,
            x.Description,
            x.Unit,
            x.UnitPrice,
            x.IsActive
        }));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateServiceRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateServiceCommand(
            request.ServiceCategory,
            request.Name,
            request.IsActive,
            request.Description,
            request.Unit,
            request.UnitPrice);

        try
        {
            var service = await _serviceService.UpdateAsync(
                id,
                command,
                cancellationToken);

            return Ok(new
            {
                service.Id,
                service.Category,
                service.Name,
                service.Description,
                service.Unit,
                service.UnitPrice,
                service.IsActive
            });
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPut("{id:guid}/activate")]
    public async Task<IActionResult> Activate(
        Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            var service = await _serviceService.ActivateAsync(
                id,
                cancellationToken);

            return Ok(new
            {
                service.Id,
                service.IsActive
            });
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPut("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(
        Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            var service = await _serviceService.DeactivateAsync(
                id,
                cancellationToken);

            return Ok(new
            {
                service.Id,
                service.IsActive
            });
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}