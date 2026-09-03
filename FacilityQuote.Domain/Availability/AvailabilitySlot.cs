using FacilityQuote.Domain.Requests;

namespace FacilityQuote.Domain.Availability;

public class AvailabilitySlot
{
    public Guid Id { get; private set; }

    public DateOnly Date { get; private set; }

    public bool MorningAvailable { get; private set; }

    public bool AfternoonAvailable { get; private set; }

    private AvailabilitySlot()
    {
    }

    public AvailabilitySlot(
        DateOnly date,
        bool morningAvailable,
        bool afternoonAvailable)
    {
        Id = Guid.NewGuid();

        Date = date;
        MorningAvailable = morningAvailable;
        AfternoonAvailable = afternoonAvailable;
    }

    public void Update(
        bool morningAvailable,
        bool afternoonAvailable)
    {
        MorningAvailable = morningAvailable;
        AfternoonAvailable = afternoonAvailable;
    }

    public bool IsAvailable(RequestTimeSlot timeSlot)
    {
        return timeSlot switch
        {
            RequestTimeSlot.Morning => MorningAvailable,
            RequestTimeSlot.Afternoon => AfternoonAvailable,
            _ => false
        };
    }
}