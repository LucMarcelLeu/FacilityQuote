using FacilityQuote.Domain.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FacilityQuote.Infrastructure.Persistence;

public class CustomerConfiguration
    : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FirstName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.LastName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.CompanyName)
            .HasMaxLength(200);

        builder.Property(x => x.Email)
            .HasMaxLength(320)
            .IsRequired();

        builder.Property(x => x.Phone)
            .HasMaxLength(50);

        builder.ComplexProperty(x => x.Address, address =>
        {
            address.Property(x => x.Street)
                .HasMaxLength(200)
                .IsRequired();

            address.Property(x => x.PostalCode)
                .HasMaxLength(20)
                .IsRequired();

            address.Property(x => x.City)
                .HasMaxLength(100)
                .IsRequired();
        });
    }
}