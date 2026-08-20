using FacilityQuote.Domain.Customers;
using FacilityQuote.Domain.Locations;
using FacilityQuote.Domain.Requests;
using FacilityQuote.Domain.Services;

namespace FacilityQuote.Tests.Requests;

public class RequestTests
{
    [Fact]
    public void Constructor_ShouldCreateNewRequest()
    {
        // Arrange
        var customer = CreateCustomer();

        var service = new Service(
            ServiceCategory.Cleaning,
            "Unterhaltsreinigung");

        var desiredDate = new DateOnly(2026, 8, 29);
        var earliestTime = new TimeOnly(9, 0);
        var latestTime = new TimeOnly(18, 0);

        // Act
        var request = new Request(
            customer,
            service,
            desiredDate,
            earliestTime,
            latestTime,
            GetLocation(),
            "Wohnung reinigen");

        // Assert
        Assert.NotEqual(Guid.Empty, request.Id);

        Assert.Equal(customer, request.Customer);
        Assert.Equal(service.Id, request.ServiceId);
        Assert.Equal(service, request.Service);

        Assert.Equal(desiredDate, request.DesiredDate);
        Assert.Equal(earliestTime, request.EarliestTime);
        Assert.Equal(latestTime, request.LatestTime);

        // var location = GetLocation();
        // Assert.Equal(location, request.Location);

        Assert.Equal("Wohnung reinigen", request.Description);

        Assert.Equal(RequestStatus.New, request.Status);
        Assert.NotEqual(default, request.CreatedAt);
    }

    [Fact]
    public void Constructor_ShouldRejectInvalidTimeRange()
    {
        // Arrange
        var customer = CreateCustomer();

        var service = new Service(
            ServiceCategory.Cleaning,
            "Unterhaltsreinigung");

        var desiredDate = new DateOnly(2026, 8, 29);
        var earliestTime = new TimeOnly(18, 0);
        var latestTime = new TimeOnly(9, 0);

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            new Request(
                customer,
                service,
                desiredDate,
                earliestTime,
                latestTime,
                GetLocation(),
                null));
    }

    private static Address GetLocation()
    {
        return new Address(
            "Bahnhofstrasse 10",
            "8001",
            "Zürich");
    }

    [Fact]
    public void StartReview_ShouldChangeStatusToReviewing()
    {
        // Arrange
        var request = CreateRequest();

        // Act
        request.StartReview();

        // Assert
        Assert.Equal(RequestStatus.Reviewing, request.Status);
    }

    [Fact]
    public void MarkQuotationCreated_ShouldChangeStatus()
    {
        // Arrange
        var request = CreateRequest();

        // Act
        request.MarkQuotationCreated();

        // Assert
        Assert.Equal(RequestStatus.QuotationCreated, request.Status);
    }

    [Fact]
    public void Reject_ShouldChangeStatusToRejected()
    {
        // Arrange
        var request = CreateRequest();

        // Act
        request.Reject();

        // Assert
        Assert.Equal(RequestStatus.Rejected, request.Status);
    }

    [Fact]
    public void Constructor_ShouldRejectEqualTimes()
    {
        // Arrange
        var customer = CreateCustomer();

        var service = new Service(
            ServiceCategory.Cleaning,
            "Unterhaltsreinigung");

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            new Request(
                customer,
                service,
                new DateOnly(2026, 8, 29),
                new TimeOnly(10, 0),
                new TimeOnly(10, 0),
                GetLocation(),
                null));
    }

    private static Customer CreateCustomer()
    {
        return new Customer(
            "Max",
            "Muster",
            null,
            new Domain.Locations.Address("Bahnhofstrasse 10", "8001", "Zürich"),
            "max@example.com",
            "+41 79 123 45 67");
    }

    private static Request CreateRequest()
    {
        var customer = CreateCustomer();

        var service = new Service(
            ServiceCategory.Cleaning,
            "Unterhaltsreinigung");

        return new Request(
            customer,
            service,
            new DateOnly(2026, 8, 29),
            new TimeOnly(9, 0),
            new TimeOnly(18, 0),
            GetLocation(),
            "Wohnung reinigen");
    }
}