using FacilityQuote.Api.Models.Requests;
using FacilityQuote.Application.Availability;
using FacilityQuote.Application.Requests;

using Microsoft.AspNetCore.Mvc;

namespace FacilityQuote.Api.Controllers;

[ApiController]
[Route("api/requests")]
public class RequestsController : ControllerBase
{
    private readonly RequestService _requestService;
    private readonly AvailabilityService _availabilityService;

    public RequestsController(
        RequestService requestService,
        AvailabilityService availabilityService)
    {
        _requestService = requestService;
        _availabilityService = availabilityService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateRequestRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateRequestCommand(
            request.FirstName,
            request.LastName,
            request.CompanyName,

            request.CustomerStreet,
            request.CustomerPostalCode,
            request.CustomerCity,

            request.Email,
            request.Phone,

            request.LocationStreet,
            request.LocationPostalCode,
            request.LocationCity,

            request.ServiceId,
            request.Quantity,

            request.DesiredDate,
            request.EarliestTime,
            request.LatestTime,

            request.Description);

        var createdRequest = await _requestService.CreateAsync(
            command,
            cancellationToken);

        return Ok(new
        {
            createdRequest.Id,
            createdRequest.Status
        });
    }

    [HttpGet("available-dates")]
    public async Task<IActionResult> GetAvailableDates(
        DateOnly from,
        DateOnly to,
        CancellationToken cancellationToken)
    {
        if (from > to)
        {
            return BadRequest(
                "The 'from' date must be before or equal to the 'to' date.");
        }

        var availability = await _availabilityService.GetRangeAsync(
            from,
            to,
            cancellationToken);

        return Ok(availability.Select(x => new
        {
            x.Date,
            x.MorningAvailable,
            x.AfternoonAvailable
        }));
    }
}