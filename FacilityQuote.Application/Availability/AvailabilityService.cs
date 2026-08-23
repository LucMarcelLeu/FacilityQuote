using FacilityQuote.Domain.Availability;

namespace FacilityQuote.Application.Availability;

public class AvailabilityService
{
    private readonly IAvailabilityRepository _repository;

    public AvailabilityService(
        IAvailabilityRepository repository) 
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<AvailabilitySlot>> GetRangeAsync(
        DateOnly from,
        DateOnly to,
        CancellationToken cancellationToken = default)
    {
        return await _repository.GetRangeAsync(
            from,
            to,
            cancellationToken);
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