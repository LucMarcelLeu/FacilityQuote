using FacilityQuote.Domain.Availability;
using FacilityQuote.Domain.Customers;
using FacilityQuote.Domain.Quotes;
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

    public DbSet<Quote> Quotes => Set<Quote>();
    public DbSet<QuoteItem> QuoteItems => Set<QuoteItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(FacilityQuoteDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}