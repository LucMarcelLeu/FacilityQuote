using FacilityQuote.Domain.Services;

namespace FacilityQuote.Tests.Services;

public class ServiceTests
{
    [Fact]
    public void Constructor_ShouldCreateActiveService()
    {
        // Arrange & Act
        Service service = GetService();

        // Assert
        Assert.NotEqual(Guid.Empty, service.Id);
        Assert.Equal(ServiceCategory.Cleaning, service.Category);
        Assert.Equal("Unterhaltsreinigung", service.Name);
        Assert.Equal(
            "Regelmässige Reinigung von Wohnungen und Büros",
            service.Description);
        Assert.True(service.IsActive);
    }

    private static Service GetService()
    {
        return new Service(
            ServiceCategory.Cleaning,
            "Unterhaltsreinigung",
            true,
            "m2",
            12,
            "Regelmässige Reinigung von Wohnungen und Büros");
    }
}