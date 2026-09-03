using FacilityQuote.Domain.Availability;
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

    public Guid AvailabilitySlotId { get; private set; }

    public AvailabilitySlot AvailabilitySlot { get; private set; } = null!;

    public RequestTimeSlot TimeSlot { get; private set; }

    public DateOnly DesiredDate { get; private set; }

    public TimeOnly EarliestTime { get; private set; }

    public TimeOnly LatestTime { get; private set; }

    public Address Location { get; private set; } = null!;

    public string? Description { get; private set; }

    public decimal? Quantity { get; set; }

    public RequestStatus Status { get; private set; }

    public DateTime CreatedAt { get; private set; }

    private Request()
    {
    }

    public Request(
        Customer customer,
        Service service,
        AvailabilitySlot availabilitySlot,
        RequestTimeSlot timeSlot,
        DateOnly desiredDate,
        TimeOnly earliestTime,
        TimeOnly latestTime,
        Address location,
        string? description,
        decimal? quantity)
    {
        if (earliestTime >= latestTime)
            throw new ArgumentException(
                "Earliest time must be before latest time.");

        if (availabilitySlot.Date != desiredDate)
            throw new ArgumentException(
                "The availability slot does not match the desired date.");

        Id = Guid.NewGuid();

        CustomerId = customer.Id;
        Customer = customer;

        ServiceId = service.Id;
        Service = service;

        AvailabilitySlotId = availabilitySlot.Id;
        AvailabilitySlot = availabilitySlot;

        TimeSlot = timeSlot;

        DesiredDate = desiredDate;
        EarliestTime = earliestTime;
        LatestTime = latestTime;

        Location = location;

        Description = description;
        Quantity = quantity;

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