using FacilityQuote.Domain.Quotes;
using FacilityQuote.Domain.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FacilityQuote.Infrastructure.Persistence;

public class QuoteConfiguration : IEntityTypeConfiguration<Quote>
{
    public void Configure(EntityTypeBuilder<Quote> builder)
    {
        builder.ToTable("Quotes");

        builder.HasKey(q => q.Id);

        builder.Property(q => q.QuoteNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(q => q.Status)
            .IsRequired();

        builder.Property(q => q.CreatedAt)
            .IsRequired();

        builder.Property(q => q.Notes)
            .HasMaxLength(2000);

        builder.Property(q => q.TravelCost)
            .HasPrecision(10, 2);

        builder.HasIndex(q => q.RequestId)
            .IsUnique();

        builder.HasOne<Request>()
            .WithOne()
            .HasForeignKey<Quote>(q => q.RequestId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(q => q.Items)
            .WithOne(i => i.Quote)
            .HasForeignKey(i => i.QuoteId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(q => q.Subtotal);
        builder.Ignore(q => q.Total);
    }
}