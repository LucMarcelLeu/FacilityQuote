using FacilityQuote.Application.Requests;
using FacilityQuote.Domain.Availability;
using FacilityQuote.Domain.Requests;

namespace FacilityQuote.Application.Availability;

public class AvailabilityService
{
    private readonly IAvailabilityRepository _repository;
    private readonly IRequestRepository _requestRepository;
    public AvailabilityService(
        IAvailabilityRepository repository,
        IRequestRepository requestRepository) 
    {
        _repository = repository;
        _requestRepository = requestRepository;
    }

    public async Task<IReadOnlyList<AvailabilityResult>> GetRangeAsync(
        DateOnly from,
        DateOnly to,
        CancellationToken cancellationToken = default)
    {
        var availability = await _repository.GetRangeAsync(
            from,
            to,
            cancellationToken);

        var requests = await _requestRepository.GetByDateRangeAsync(
            from,
            to,
            cancellationToken);

        return availability
            .Select(slot =>
            {
                var bookedMorning = requests.Any(
                    r => r.AvailabilitySlotId == slot.Id &&
                         r.TimeSlot == RequestTimeSlot.Morning);

                var bookedAfternoon = requests.Any(
                    r => r.AvailabilitySlotId == slot.Id &&
                         r.TimeSlot == RequestTimeSlot.Afternoon);

                return new AvailabilityResult(
                    slot.Date,
                    slot.MorningAvailable,
                    slot.AfternoonAvailable,
                    bookedMorning,
                    bookedAfternoon);
            })
            .ToList();
    }

    public async Task<AvailabilitySlot> CreateAsync(
        DateOnly date,
        bool morningAvailable,
        bool afternoonAvailable,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByDateAsync(
            date,
            cancellationToken);

        if (existing is not null)
        {
            throw new InvalidOperationException(
                $"Availability for '{date}' already exists.");
        }

        var availability = new AvailabilitySlot(
            date,
            morningAvailable,
            afternoonAvailable);

        await _repository.AddAsync(
            availability,
            cancellationToken);

        return availability;
    }

    public async Task<AvailabilitySlot> UpdateAsync(
        DateOnly date,
        bool morningAvailable,
        bool afternoonAvailable,
        CancellationToken cancellationToken = default)
    {
        var availability = await _repository.GetByDateAsync(
            date,
            cancellationToken);

        if (availability is null)
        {
            throw new InvalidOperationException(
                $"Availability for '{date}' was not found.");
        }

        availability.Update(
            morningAvailable,
            afternoonAvailable);

        await _repository.SaveChangesAsync(
            cancellationToken);

        return availability;
    }

    public async Task<AvailabilitySlot> SetAsync(
        DateOnly date,
        bool morningAvailable,
        bool afternoonAvailable,
        CancellationToken cancellationToken = default)
    {
        var availability = await _repository.GetByDateAsync(
            date,
            cancellationToken);

        if (availability is null)
        {
            availability = new AvailabilitySlot(
                date,
                morningAvailable,
                afternoonAvailable);

            await _repository.AddAsync(
                availability,
                cancellationToken);

            return availability;
        }

        availability.Update(
            morningAvailable,
            afternoonAvailable);

        await _repository.SaveChangesAsync(
            cancellationToken);

        return availability;
    }
}