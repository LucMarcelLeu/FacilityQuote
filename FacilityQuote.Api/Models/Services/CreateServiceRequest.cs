using FacilityQuote.Domain.Services;

namespace FacilityQuote.Api.Models.Services;

public sealed record CreateServiceRequest(

    ServiceCategory ServiceCategory,

    string Name,

    bool IsActive,

    string? Description,

    string Unit,

    decimal UnitPrice);