using FacilityQuote.Domain.Requests;

namespace FacilityQuote.Application.Requests.Dtos;

public class UpdateRequestStatusDto
{
    public RequestStatus Status { get; set; }
}