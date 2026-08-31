using FacilityQuote.Application.Quotes;
using FacilityQuote.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace FacilityQuote.Infrastructure.Pdf;

public class QuotePdfService(
    IQuoteRepository quoteRepository,
    IOptions<CompanyOptions> companyOptions)
    : IQuotePdfService
{
    private readonly CompanyOptions company = companyOptions.Value;

    public async Task<QuotePdfResult> GenerateAsync(
        Guid quoteId,
        CancellationToken cancellationToken = default)
    {
        var result = await quoteRepository.GetForPdfAsync(
            quoteId,
            cancellationToken);

        if (result is null)
        {
            throw new KeyNotFoundException(
                $"Quote '{quoteId}' was not found.");
        }

        var (quote, request) = result.Value;

        var logo = LoadLogo();

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.MarginHorizontal(45);
                page.MarginTop(40);
                page.MarginBottom(45);

                page.DefaultTextStyle(style =>
                    style.FontSize(10));

                page.Header()
                    .Column(header =>
                    {
                        header.Item()
                            .Row(row =>
                            {
                                row.RelativeItem()
                                    .Row(companyRow =>
                                    {
                                        if (logo is not null)
                                        {
                                            companyRow.ConstantItem(70)
                                                .AlignMiddle()
                                                .Image(logo)
                                                .FitArea();
                                        }

                                        companyRow.RelativeItem()
                                            .PaddingLeft(
                                                logo is not null ? 12 : 0)
                                            .Column(companyColumn =>
                                            {
                                                companyColumn.Item()
                                                    .Text(company.Name)
                                                    .Bold()
                                                    .FontSize(16);

                                                companyColumn.Item()
                                                    .PaddingTop(3)
                                                    .Text(
                                                        $"{company.Street}, " +
                                                        $"{company.PostalCode} " +
                                                        $"{company.City}")
                                                    .FontSize(8);

                                                companyColumn.Item()
                                                    .PaddingTop(2)
                                                    .Text(
                                                        $"{company.Phone} · " +
                                                        $"{company.Email}")
                                                    .FontSize(8);
                                            });
                                    });

                                row.ConstantItem(190)
                                    .AlignRight()
                                    .Column(info =>
                                    {
                                        info.Item()
                                            .Text("OFFERT")
                                            .Bold()
                                            .FontSize(9);

                                        info.Item()
                                            .PaddingTop(3)
                                            .Text(quote.QuoteNumber)
                                            .Bold()
                                            .FontSize(15);

                                        info.Item()
                                            .PaddingTop(2)
                                            .Text(
                                                quote.CreatedAt
                                                    .ToLocalTime()
                                                    .ToString("dd.MM.yyyy"))
                                            .FontSize(9);
                                    });
                            });

                        header.Item()
                            .PaddingTop(14)
                            .LineHorizontal(1);
                    });

                page.Content()
                    .PaddingTop(28)
                    .Column(column =>
                    {
                        column.Spacing(24);

                        column.Item()
                            .Column(title =>
                            {
                                title.Item()
                                    .Text("Offerte")
                                    .Bold()
                                    .FontSize(26);

                                title.Item()
                                    .PaddingTop(5)
                                    .Text(
                                        "Vielen Dank für Ihre Anfrage. " +
                                        "Gerne unterbreiten wir Ihnen " +
                                        "folgendes Angebot.")
                                    .FontSize(10);
                            });

                        column.Item()
                            .Row(row =>
                            {
                                row.RelativeItem()
                                    .Column(customer =>
                                    {
                                        customer.Item()
                                            .Text("KUNDE")
                                            .Bold()
                                            .FontSize(8);

                                        customer.Item()
                                            .PaddingTop(6)
                                            .Text(
                                                $"{request.Customer.FirstName} " +
                                                $"{request.Customer.LastName}")
                                            .Bold()
                                            .FontSize(11);

                                        customer.Item()
                                            .PaddingTop(3)
                                            .Text(
                                                request.Customer.Email);

                                        if (!string.IsNullOrWhiteSpace(
                                                request.Customer.Phone))
                                        {
                                            customer.Item()
                                                .Text(
                                                    request.Customer.Phone);
                                        }
                                    });

                                row.RelativeItem()
                                    .Column(location =>
                                    {
                                        location.Item()
                                            .Text("EINSATZORT")
                                            .Bold()
                                            .FontSize(8);

                                        location.Item()
                                            .PaddingTop(6)
                                            .Text(
                                                request.Location.Street)
                                            .Bold()
                                            .FontSize(11);

                                        location.Item()
                                            .PaddingTop(3)
                                            .Text(
                                                $"{request.Location.PostalCode} " +
                                                $"{request.Location.City}");
                                    });
                            });

                        column.Item()
                            .Row(row =>
                            {
                                row.RelativeItem()
                                    .Column(service =>
                                    {
                                        service.Item()
                                            .Text("DIENSTLEISTUNG")
                                            .Bold()
                                            .FontSize(8);

                                        service.Item()
                                            .PaddingTop(5)
                                            .Text(
                                                request.Service.Name)
                                            .Bold();
                                    });

                                row.RelativeItem()
                                    .Column(appointment =>
                                    {
                                        appointment.Item()
                                            .Text("GEWÜNSCHTER TERMIN")
                                            .Bold()
                                            .FontSize(8);

                                        appointment.Item()
                                            .PaddingTop(5)
                                            .Text(
                                                $"{request.DesiredDate:dd.MM.yyyy} · " +
                                                $"{request.EarliestTime:HH\\:mm} – " +
                                                $"{request.LatestTime:HH\\:mm}");
                                    });
                            });

                        column.Item()
                            .Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(4.5f);
                                    columns.RelativeColumn(1.2f);
                                    columns.RelativeColumn(1.6f);
                                    columns.RelativeColumn(1.7f);
                                });

                                table.Header(header =>
                                {
                                    header.Cell()
                                        .PaddingBottom(8)
                                        .Text("LEISTUNG")
                                        .Bold()
                                        .FontSize(8);

                                    header.Cell()
                                        .PaddingBottom(8)
                                        .AlignRight()
                                        .Text("MENGE")
                                        .Bold()
                                        .FontSize(8);

                                    header.Cell()
                                        .PaddingBottom(8)
                                        .AlignRight()
                                        .Text("EINZELPREIS")
                                        .Bold()
                                        .FontSize(8);

                                    header.Cell()
                                        .PaddingBottom(8)
                                        .AlignRight()
                                        .Text("BETRAG")
                                        .Bold()
                                        .FontSize(8);

                                    header.Cell()
                                        .ColumnSpan(4)
                                        .LineHorizontal(1);
                                });

                                foreach (var item in quote.Items)
                                {
                                    table.Cell()
                                        .PaddingVertical(9)
                                        .Text(item.Description)
                                        .Bold();

                                    table.Cell()
                                        .PaddingVertical(9)
                                        .AlignRight()
                                        .Text(
                                            $"{item.Quantity:0.##} {item.Unit}");

                                    table.Cell()
                                        .PaddingVertical(9)
                                        .AlignRight()
                                        .Text(
                                            FormatCurrency(
                                                item.UnitPrice));

                                    table.Cell()
                                        .PaddingVertical(9)
                                        .AlignRight()
                                        .Text(
                                            FormatCurrency(
                                                item.Total))
                                        .Bold();

                                    table.Cell()
                                        .ColumnSpan(4)
                                        .LineHorizontal(0.5f);
                                }
                            });

                        column.Item()
                            .AlignRight()
                            .Column(totals =>
                            {
                                totals.Item()
                                    .Row(row =>
                                    {
                                        row.ConstantItem(150)
                                            .Text("Zwischensumme");

                                        row.ConstantItem(110)
                                            .AlignRight()
                                            .Text(
                                                FormatCurrency(
                                                    quote.Subtotal));
                                    });

                                totals.Item()
                                    .PaddingTop(6)
                                    .Row(row =>
                                    {
                                        row.ConstantItem(150)
                                            .Text("Anfahrt");

                                        row.ConstantItem(110)
                                            .AlignRight()
                                            .Text(
                                                FormatCurrency(
                                                    quote.TravelCost));
                                    });

                                totals.Item()
                                    .PaddingTop(10)
                                    .LineHorizontal(1);

                                totals.Item()
                                    .PaddingTop(10)
                                    .Row(row =>
                                    {
                                        row.ConstantItem(150)
                                            .Text("TOTAL")
                                            .Bold()
                                            .FontSize(12);

                                        row.ConstantItem(110)
                                            .AlignRight()
                                            .Text(
                                                FormatCurrency(
                                                    quote.Total))
                                            .Bold()
                                            .FontSize(14);
                                    });
                            });

                        if (quote.ValidUntil.HasValue)
                        {
                            column.Item()
                                .Column(validity =>
                                {
                                    validity.Item()
                                        .Text("GÜLTIGKEIT")
                                        .Bold()
                                        .FontSize(8);

                                    validity.Item()
                                        .PaddingTop(5)
                                        .Text(
                                            $"Diese Offerte ist gültig bis " +
                                            $"{quote.ValidUntil.Value:dd.MM.yyyy}.");
                                });
                        }

                        if (!string.IsNullOrWhiteSpace(quote.Notes))
                        {
                            column.Item()
                                .Column(notes =>
                                {
                                    notes.Item()
                                        .Text("BEMERKUNGEN")
                                        .Bold()
                                        .FontSize(8);

                                    notes.Item()
                                        .PaddingTop(5)
                                        .Text(quote.Notes);
                                });
                        }

                        column.Item()
                            .PaddingTop(10)
                            .Text(
                                "Vielen Dank für Ihr Vertrauen. " +
                                "Wir freuen uns auf Ihren Auftrag.");
                    });

                page.Footer()
                    .PaddingTop(10)
                    .BorderTop(0.5f)
                    .Row(row =>
                    {
                        row.RelativeItem()
                            .Text(
                                $"{company.Name} · " +
                                $"{company.Street} · " +
                                $"{company.PostalCode} {company.City}")
                            .FontSize(8);

                        row.ConstantItem(180)
                            .AlignRight()
                            .Column(contact =>
                            {
                                contact.Item()
                                    .AlignRight()
                                    .Text(
                                        $"{company.Phone} · " +
                                        company.Email)
                                    .FontSize(8);

                                contact.Item()
                                    .PaddingTop(2)
                                    .AlignRight()
                                    .Text(text =>
                                    {
                                        text.Span("Seite ")
                                            .FontSize(8);

                                        text.CurrentPageNumber()
                                            .FontSize(8);

                                        text.Span(" / ")
                                            .FontSize(8);

                                        text.TotalPages()
                                            .FontSize(8);
                                    });
                            });
                    });
            });
        });

        var pdf = document.GeneratePdf();

        return new QuotePdfResult(
            pdf,
            $"{quote.QuoteNumber}.pdf");
    }

    private byte[]? LoadLogo()
    {
        if (string.IsNullOrWhiteSpace(company.LogoPath))
        {
            return null;
        }

        var path = Path.Combine(
            AppContext.BaseDirectory,
            company.LogoPath);

        if (!File.Exists(path))
        {
            return null;
        }

        return File.ReadAllBytes(path);
    }

    private static string FormatCurrency(decimal value)
    {
        return $"CHF {value:N2}";
    }
}