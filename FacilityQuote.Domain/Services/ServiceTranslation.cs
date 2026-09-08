using FacilityQuote.Domain.Requests;

namespace FacilityQuote.Domain.Services;

public class ServiceTranslation
{
    public Guid Id { get; private set; }

    public Guid ServiceId { get; private set; }

    public Service Service { get; private set; } = null!;

    public RequestLanguage Language { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    private ServiceTranslation()
    {
    }

    public ServiceTranslation(
        Service service,
        RequestLanguage language,
        string name,
        string? description = null)
    {
        Id = Guid.NewGuid();

        Service = service;
        ServiceId = service.Id;

        Language = language;
        Name = name;
        Description = description;
    }

    public void Update(
        string name,
        string? description)
    {
        Name = name;
        Description = description;
    }
}