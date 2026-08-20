using FacilityQuote.Domain.Services;

namespace FacilityQuote.Application.Services;

public sealed record CreateServiceCommand(
    ServiceCategory ServiceCategory,
    string Name,
    bool IsActive,
    string? Description);