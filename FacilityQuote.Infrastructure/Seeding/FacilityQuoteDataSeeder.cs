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
                "m2",
                13,
                "Regelmässige Reinigung von Wohnungen und Büros"),
            new Service(
                ServiceCategory.Cleaning,
                "Endreinigung",
                true,
                "m2",
                11,
                "Wohnungsreinigung bei Wohnungswechsel"),

            new Service(
                ServiceCategory.Cleaning,
                "Fensterreinigung",
                true,
                "Anzahl",
                8,
                "Reinigung von Fenstern und Glasflächen"),

            new Service(
                ServiceCategory.Clearance,
                "Wohnungsräumung",
                true,
                "m3",
                16,
                "Komplette Räumung von Wohnungen"),

            new Service(
                ServiceCategory.Clearance,
                "Keller- und Estrichräumung",
                true,
                "m3",
                18,
                "Räumung von Keller und Estrich"),

            new Service(
                ServiceCategory.Clearance,
                "Entsorgung",
                true,
                "m3",
                33,
                "Entsorgung von Möbeln und Sperrgut"),

            new Service(
                ServiceCategory.Gardening,
                "Rasenpflege",
                true,
                "m2",
                4,
                "Rasen mähen und Pflege"),

            new Service(
                ServiceCategory.Gardening,
                "Heckenschnitt",
                true,
                "Laufmeter",
                6,
                "Schneiden und Formen von Hecken"),

            new Service(
                ServiceCategory.Gardening,
                "Gartenräumung",
                true,
                "m3",
                9,
                "Allgemeine Räumungs- und Gartenarbeiten")
        };

        context.Services.AddRange(services);
        context.SaveChanges();
    }
}