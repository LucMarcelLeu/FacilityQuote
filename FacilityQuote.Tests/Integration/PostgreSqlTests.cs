using FacilityQuote.Domain.Customers;
using FacilityQuote.Domain.Locations;
using FacilityQuote.Domain.Requests;
using FacilityQuote.Domain.Services;
using FacilityQuote.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FacilityQuote.Tests.Integration;

public class PostgreSqlTests
{
    [Fact]
    public async Task Should_Save_And_Load_Request()
    {
        var options = new DbContextOptionsBuilder<FacilityQuoteDbContext>()
            .UseNpgsql(
                "Host=localhost;" +
                "Port=5432;" +
                "Database=facilityquote;" +
                "Username=facilityquote;" +
                "Password=facilityquote")
            .Options;

        var customer = new Customer(
            "Max",
            "Muster",
            "Muster AG",
            new Domain.Locations.Address("Bahnhofstrasse 10", "8001", "Zürich"),
            "max@muster.ch",
            "+41 79 123 45 67");

        var service = new Service(
            ServiceCategory.Cleaning,
            "Unterhaltsreinigung");

        var location = new Address(
            "Seestrasse 100",
            "8002",
            "Zürich");

        var request = new Request(
            customer,
            service,
            new DateOnly(2026, 8, 29),
            new TimeOnly(9, 0),
            new TimeOnly(18, 0),
            location,
            "Wohnung reinigen");

        await using (var context = new FacilityQuoteDbContext(options))
        {
            context.Customers.Add(customer);
            context.Services.Add(service);
            context.Requests.Add(request);

            await context.SaveChangesAsync();
        }

        await using (var context = new FacilityQuoteDbContext(options))
        {
            var loadedRequest = await context.Requests
                .Include(x => x.Customer)
                .Include(x => x.Service)
                .SingleAsync(x => x.Id == request.Id);

            Assert.Equal(request.Id, loadedRequest.Id);

            Assert.Equal(
                "Max",
                loadedRequest.Customer.FirstName);

            Assert.Equal(
                "Unterhaltsreinigung",
                loadedRequest.Service.Name);

            Assert.Equal(
                new DateOnly(2026, 8, 29),
                loadedRequest.DesiredDate);

            Assert.Equal(
                new TimeOnly(9, 0),
                loadedRequest.EarliestTime);

            Assert.Equal(
                new TimeOnly(18, 0),
                loadedRequest.LatestTime);

            Assert.Equal(
                "Seestrasse 100",
                loadedRequest.Location.Street);

            Assert.Equal(
                "8002",
                loadedRequest.Location.PostalCode);

            Assert.Equal(
                "Zürich",
                loadedRequest.Location.City);
        }
    }
}