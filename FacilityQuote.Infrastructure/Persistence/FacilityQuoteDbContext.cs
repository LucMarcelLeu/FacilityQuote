using FacilityQuote.Domain.Availability;
using FacilityQuote.Domain.Customers;
using FacilityQuote.Domain.Requests;
using FacilityQuote.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace FacilityQuote.Infrastructure.Persistence;

public class FacilityQuoteDbContext : DbContext
{
    public FacilityQuoteDbContext(
        DbContextOptions<FacilityQuoteDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();

    public DbSet<Service> Services => Set<Service>();

    public DbSet<Request> Requests => Set<Request>();

    public DbSet<AvailabilitySlot> Availabilities => Set<AvailabilitySlot>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(FacilityQuoteDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}