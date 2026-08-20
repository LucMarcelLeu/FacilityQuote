using FacilityQuote.Domain.Services;
using FacilityQuote.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FacilityQuote.Infrastructure.Seeding;

public static class FacilityQuoteDataSeeder
{
    public static void Seed(FacilityQuoteDbContext context)
    {
        if (context.Services.Any())
        {
            return;
        }

        var services = new[]
        {
            new Service(
                ServiceCategory.Cleaning,
                "Unterhaltsreinigung",
                true,
                "Regelmässige Reinigung von Wohnungen und Büros"),

            new Service(
                ServiceCategory.Cleaning,
                "Endreinigung",
                true,
                "Wohnungsreinigung bei Wohnungswechsel"),

            new Service(
                ServiceCategory.Cleaning,
                "Fensterreinigung",
                true,
                "Reinigung von Fenstern und Glasflächen"),

            new Service(
                ServiceCategory.Clearance,
                "Wohnungsräumung",
                true,
                "Komplette Räumung von Wohnungen"),

            new Service(
                ServiceCategory.Clearance,
                "Keller- und Estrichräumung",
                true,
                "Räumung von Keller und Estrich"),

            new Service(
                ServiceCategory.Clearance,
                "Entsorgung",
                true,
                "Entsorgung von Möbeln und Sperrgut"),

            new Service(
                ServiceCategory.Gardening,
                "Rasenpflege",
                true,
                "Rasen mähen und Pflege"),

            new Service(
                ServiceCategory.Gardening,
                "Heckenschnitt",
                true,
                "Schneiden und Formen von Hecken"),

            new Service(
                ServiceCategory.Gardening,
                "Gartenräumung",
                true,
                "Allgemeine Räumungs- und Gartenarbeiten")
        };

        context.Services.AddRange(services);
        context.SaveChanges();
    }
}