using FacilityQuote.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FacilityQuote.Infrastructure.Persistence;

public class ServiceTranslationConfiguration
    : IEntityTypeConfiguration<ServiceTranslation>
{
    public void Configure(
        EntityTypeBuilder<ServiceTranslation> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Language)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.HasIndex(x => new
        {
            x.ServiceId,
            x.Language
        })
        .IsUnique();
    }
}