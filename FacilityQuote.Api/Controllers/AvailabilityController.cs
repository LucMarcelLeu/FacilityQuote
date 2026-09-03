using FacilityQuote.Api.Models.Availability;
using FacilityQuote.Application.Availability;
using Microsoft.AspNetCore.Mvc;

namespace FacilityQuote.Api.Controllers;

[ApiController]
[Route("api/availability")]
public class AvailabilityController : ControllerBase
{
    private readonly AvailabilityService _availabilityService;

    public AvailabilityController(
        AvailabilityService availabilityService)
    {
        _availabilityService = availabilityService;
    }

    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] DateOnly from,
        [FromQuery] DateOnly to,
        CancellationToken cancellationToken)
    {
        if (from > to)
        {
            return BadRequest(
                "The 'from' date must be before or equal to the 'to' date.");
        }

        var result =
            await _availabilityService.GetRangeAsync(
                from,
                to,
                cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateAvailabilityRequest request,
        CancellationToken cancellationToken)
    {
        var availability = await _availabilityService.CreateAsync(
            request.Date,
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

    [HttpPut("{date}")]
    public async Task<IActionResult> Update(
        DateOnly date,
        UpdateAvailabilityRequest request,
        CancellationToken cancellationToken)
    {
        var availability = await _availabilityService.UpdateAsync(
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