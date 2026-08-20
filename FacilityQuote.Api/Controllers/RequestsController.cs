using FacilityQuote.Api.Models.Requests;
using FacilityQuote.Application.Requests;
using Microsoft.AspNetCore.Mvc;

namespace FacilityQuote.Api.Controllers;

[ApiController]
[Route("api/requests")]
public class RequestsController : ControllerBase
{
    private readonly RequestService _requestService;

    public RequestsController(RequestService requestService)
    {
        _requestService = requestService;
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
}