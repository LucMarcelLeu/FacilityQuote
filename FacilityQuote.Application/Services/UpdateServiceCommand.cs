using FacilityQuote.Domain.Services;

namespace FacilityQuote.Application.Services;

public sealed record UpdateServiceCommand(

    ServiceCategory ServiceCategory,

    string Name,

    bool IsActive,

    string? Description,

    string Unit,

    decimal UnitPrice);