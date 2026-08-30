namespace FacilityQuote.Domain.Services;

public class Service
{
    public Guid Id { get; private set; }

    public ServiceCategory Category { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public bool IsActive { get; private set; }

    public decimal UnitPrice { get; set; }

    public string Unit { get; set; } = string.Empty;

    private Service()
    {
    }

    public Service(
        ServiceCategory category,
        string name,
        bool isActive,
        string? description = null
        )
    {
        Id = Guid.NewGuid();
        Category = category;
        Name = name;
        IsActive = isActive;
        Description = description;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }
}