using FacilityQuote.Domain.Requests;
using FacilityQuote.Domain.Services;
using FacilityQuote.Infrastructure.Persistence;

namespace FacilityQuote.Infrastructure.Seeding;

public static class FacilityQuoteDataSeeder
{
    public static void Seed(FacilityQuoteDbContext context)
    {
        SeedServices(context);
        SeedServiceTranslations(context);

        context.SaveChanges();
    }

    private static void SeedServices(FacilityQuoteDbContext context)
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
    }

    private static void SeedServiceTranslations(
        FacilityQuoteDbContext context)
    {
        var services = context.Services.ToList();

        foreach (var service in services)
        {
            AddTranslation(
                context,
                service,
                RequestLanguage.German,
                GetGermanName(service),
                GetGermanDescription(service));

            AddTranslation(
                context,
                service,
                RequestLanguage.English,
                GetEnglishName(service),
                GetEnglishDescription(service));
        }
    }

    private static void AddTranslation(
        FacilityQuoteDbContext context,
        Service service,
        RequestLanguage language,
        string name,
        string description)
    {
        var exists = context.Set<ServiceTranslation>()
            .Any(x =>
                x.ServiceId == service.Id &&
                x.Language == language);

        if (exists)
        {
            return;
        }

        context.Set<ServiceTranslation>().Add(
            new ServiceTranslation(
                service,
                language,
                name,
                description)
        );
    }

    private static string GetGermanName(Service service)
    {
        return service.Name;
    }

    private static string GetGermanDescription(Service service)
    {
        return service.Description ?? string.Empty;
    }

    private static string GetEnglishName(Service service)
    {
        return service.Name switch
        {
            "Unterhaltsreinigung" => "Regular Cleaning",
            "Endreinigung" => "End-of-Tenancy Cleaning",
            "Fensterreinigung" => "Window Cleaning",

            "Wohnungsräumung" => "Apartment Clearance",
            "Keller- und Estrichräumung" => "Basement & Attic Clearance",
            "Entsorgung" => "Disposal",

            "Rasenpflege" => "Lawn Care",
            "Heckenschnitt" => "Hedge Trimming",
            "Gartenräumung" => "Garden Clearance",

            _ => service.Name
        };
    }

    private static string GetEnglishDescription(Service service)
    {
        return service.Name switch
        {
            "Unterhaltsreinigung" =>
                "Regular cleaning of apartments and offices",

            "Endreinigung" =>
                "Cleaning of apartments when moving out",

            "Fensterreinigung" =>
                "Cleaning of windows and glass surfaces",

            "Wohnungsräumung" =>
                "Complete clearance of apartments",

            "Keller- und Estrichräumung" =>
                "Clearance of basements and attics",

            "Entsorgung" =>
                "Disposal of furniture and bulky waste",

            "Rasenpflege" =>
                "Lawn mowing and maintenance",

            "Heckenschnitt" =>
                "Trimming and shaping of hedges",

            "Gartenräumung" =>
                "General clearance and gardening work",

            _ => service.Description ?? string.Empty
        };
    }
}