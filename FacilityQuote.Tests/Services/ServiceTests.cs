using FacilityQuote.Domain.Services;

namespace FacilityQuote.Tests.Services;

public class ServiceTests
{
    [Fact]
    public void Constructor_ShouldCreateActiveService()
    {
        // Arrange & Act
        var service = new Service(
            ServiceCategory.Cleaning,
            "Unterhaltsreinigung",
            "Regelmässige Reinigung von Wohnungen und Büros");

        // Assert
        Assert.NotEqual(Guid.Empty, service.Id);
        Assert.Equal(ServiceCategory.Cleaning, service.Category);
        Assert.Equal("Unterhaltsreinigung", service.Name);
        Assert.Equal(
            "Regelmässige Reinigung von Wohnungen und Büros",
            service.Description);
        Assert.True(service.IsActive);
    }

    [Fact]
    public void Deactivate_ShouldDeactivateService()
    {
        // Arrange
        var service = new Service(
            ServiceCategory.Gardening,
            "Rasenpflege");

        // Act
        service.Deactivate();

        // Assert
        Assert.False(service.IsActive);
    }

    [Fact]
    public void Activate_ShouldActivateDeactivatedService()
    {
        // Arrange
        var service = new Service(
            ServiceCategory.Clearance,
            "Wohnungsräumung");

        service.Deactivate();

        // Act
        service.Activate();

        // Assert
        Assert.True(service.IsActive);
    }
}