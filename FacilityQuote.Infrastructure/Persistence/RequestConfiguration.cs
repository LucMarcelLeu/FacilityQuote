using FacilityQuote.Domain.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FacilityQuote.Infrastructure.Persistence;

public class RequestConfiguration : IEntityTypeConfiguration<Request>
{
    public void Configure(EntityTypeBuilder<Request> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Description)
            .HasMaxLength(2000);
            
        builder.Property(r => r.Quantity)
            .HasPrecision(10, 2);

        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.HasOne(x => x.Customer)
            .WithMany(x => x.Requests)
            .HasForeignKey(x => x.CustomerId)
            .IsRequired();

        builder.HasOne(x => x.Service)
            .WithMany()
            .HasForeignKey(x => x.ServiceId)
            .IsRequired();

        builder.ComplexProperty(x => x.Location, location =>
        {
            location.Property(x => x.Street)
                .HasMaxLength(200)
                .IsRequired();

            location.Property(x => x.PostalCode)
                .HasMaxLength(20)
                .IsRequired();

            location.Property(x => x.City)
                .HasMaxLength(100)
                .IsRequired();
        });
    }
}