using FacilityQuote.Api.Models.Availability;
using FacilityQuote.Application.Availability;
using Microsoft.AspNetCore.Mvc;

namespace FacilityQuote.Api.Controllers;

[ApiController]
[Route("api/admin/availability")]
public class AdminAvailabilityController : ControllerBase
{
    private readonly AvailabilityService _availabilityService;

    public AdminAvailabilityController(
        AvailabilityService availabilityService)
    {
        _availabilityService = availabilityService;
    }

    [HttpGet]
    public async Task<IActionResult> Get(
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

        return Ok(availability);
    }

    [HttpPut("{date}")]
    public async Task<IActionResult> Set(
        DateOnly date,
        UpdateAvailabilityRequest request,
        CancellationToken cancellationToken)
    {
        var availability = await _availabilityService.SetAsync(
            date,
            request.MorningAvailable,
            request.AfternoonAvailable,
            cancellationToken);

        return Ok(new
        {
            availability.Id,
            availability.Date,
            availability.MorningAvailable,
            availability.AfternoonAvailable
        });
    }
}