using FacilityQuote.Application.Requests;
using FacilityQuote.Application.Requests.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FacilityQuote.Api.Controllers;

[ApiController]
[Route("api/admin/requests")]
[Authorize(Roles = "admin")]
public class AdminRequestsController : ControllerBase
{
    private readonly RequestService _requestService;

    public AdminRequestsController(RequestService requestService)
    {
        _requestService = requestService;
    }

    [HttpGet("{id:guid}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var request = await _requestService.GetByIdAsync(
            id,
            cancellationToken);

        if (request is null)
        {
            return NotFound();
        }

        var dto = new RequestDto
        {
            Id = request.Id,

            CustomerId = request.CustomerId,

            CustomerName =
                $"{request.Customer.FirstName} {request.Customer.LastName}",

            Email = request.Customer.Email,
            Phone = request.Customer.Phone,

            ServiceId = request.ServiceId,
            Service = request.Service.Name,

            DesiredDate = request.DesiredDate,
            EarliestTime = request.EarliestTime,
            LatestTime = request.LatestTime,

            Street = request.Location.Street,
            PostalCode = request.Location.PostalCode,
            City = request.Location.City,

            Description = request.Description,

            Status = request.Status.ToString(),

            CreatedAt = request.CreatedAt
        };

        return Ok(dto);
    }

    [HttpPut("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(
        Guid id,
        UpdateRequestStatusDto dto,
        CancellationToken cancellationToken)
    {
        var request = await _requestService.UpdateStatusAsync(
            id,
            dto.Status,
            cancellationToken);

        if (request is null)
        {
            return NotFound();
        }

        return Ok(new
        {
            request.Id,
            request.Status
        });
    }
}