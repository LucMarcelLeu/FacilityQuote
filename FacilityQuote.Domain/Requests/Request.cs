using FacilityQuote.Domain.Customers;
using FacilityQuote.Domain.Locations;
using FacilityQuote.Domain.Services;

namespace FacilityQuote.Domain.Requests;

public class Request
{
    public Guid Id { get; private set; }

    public Guid CustomerId { get; private set; }

    public Customer Customer { get; private set; } = null!;

    public Guid ServiceId { get; private set; }

    public Service Service { get; private set; } = null!;

    public DateOnly DesiredDate { get; private set; }

    public TimeOnly EarliestTime { get; private set; }

    public TimeOnly LatestTime { get; private set; }

    public Address Location { get; private set; } = null!;

    public string? Description { get; private set; }

    public RequestStatus Status { get; private set; }

    public DateTime CreatedAt { get; private set; }

    private Request()
    {
    }

    public Request(
        Customer customer,
        Service service,
        DateOnly desiredDate,
        TimeOnly earliestTime,
        TimeOnly latestTime,
        Address location,
        string? description)
    {
        if (earliestTime >= latestTime)
            throw new ArgumentException(
                "Earliest time must be before latest time.");

        Id = Guid.NewGuid();

        CustomerId = customer.Id;
        Customer = customer;
        
        ServiceId = service.Id;
        Service = service;

        DesiredDate = desiredDate;
        EarliestTime = earliestTime;
        LatestTime = latestTime;

        Location = location;

        Description = description;

        Status = RequestStatus.New;
        CreatedAt = DateTime.UtcNow;
    }

    public void StartReview()
    {
        Status = RequestStatus.Reviewing;
    }

    public void MarkQuotationCreated()
    {
        Status = RequestStatus.QuotationCreated;
    }

    public void Reject()
    {
        Status = RequestStatus.Rejected;
    }
}