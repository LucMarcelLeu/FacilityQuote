using FacilityQuote.Domain.Availability;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FacilityQuote.Infrastructure.Persistence;

public class AvailabilityConfiguration
    : IEntityTypeConfiguration<AvailabilitySlot>
{
    public void Configure(
        EntityTypeBuilder<AvailabilitySlot> builder)
    {
        builder.ToTable("Availabilities");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Date)
            .IsRequired();

        builder.Property(x => x.MorningAvailable)
            .IsRequired();

        builder.Property(x => x.AfternoonAvailable)
            .IsRequired();

        builder.HasIndex(x => x.Date)
            .IsUnique();
    }
}