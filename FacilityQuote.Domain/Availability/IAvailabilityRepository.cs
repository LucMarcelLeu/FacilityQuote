using FacilityQuote.Domain.Availability;

public interface IAvailabilityRepository
{
    Task<AvailabilitySlot?> GetByDateAsync(
        DateOnly date,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AvailabilitySlot>> GetRangeAsync(
        DateOnly from,
        DateOnly to,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        AvailabilitySlot availability,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}