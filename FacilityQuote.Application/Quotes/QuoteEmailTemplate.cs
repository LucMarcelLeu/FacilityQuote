using System.Globalization;
using FacilityQuote.Domain.Customers;
using FacilityQuote.Domain.Quotes;

namespace FacilityQuote.Application.Quotes;

public static class QuoteEmailTemplate
{
    public static string Build(
        Quote quote,
        Customer customer)
    {
        var firstName = HtmlEncode(customer.FirstName);

        var validUntil = quote.ValidUntil.HasValue
            ? quote.ValidUntil.Value.ToString(
                "dd.MM.yyyy",
                CultureInfo.InvariantCulture)
            : "Nicht festgelegt";

        var total = quote.Total.ToString(
            "N2",
            CultureInfo.InvariantCulture);

        return $$"""
        <!DOCTYPE html>
        <html lang="de">

        <head>
            <meta charset="utf-8">
            <meta
                name="viewport"
                content="width=device-width, initial-scale=1.0">

            <title>
                Ihre Offerte {{quote.QuoteNumber}}
            </title>
        </head>

        <body style="
            margin:0;
            padding:0;
            background-color:#f3f4f6;
            font-family:Arial, Helvetica, sans-serif;
            color:#1f2937;
        ">

        <table
            width="100%"
            cellpadding="0"
            cellspacing="0"
            border="0"
            style="
                background-color:#f3f4f6;
            "
        >
            <tr>
                <td
                    align="center"
                    style="
                        padding:40px 16px;
                    "
                >

                    <table
                        width="100%"
                        cellpadding="0"
                        cellspacing="0"
                        border="0"
                        style="
                            max-width:620px;
                            background:#ffffff;
                            border-radius:12px;
                            overflow:hidden;
                            box-shadow:0 2px 12px rgba(0,0,0,0.08);
                        "
                    >

                        <!-- =====================================================
                             HEADER
                             ===================================================== -->

                        <tr>
                            <td style="
                                padding:28px 36px;
                                border-bottom:1px solid #e5e7eb;
                            ">

                                <table
                                    width="100%"
                                    cellpadding="0"
                                    cellspacing="0"
                                    border="0"
                                >
                                    <tr>

                                        <td valign="top">

                                            <div style="
                                                font-size:20px;
                                                font-weight:bold;
                                                color:#111827;
                                            ">
                                                Leu Gebäudeservice
                                            </div>

                                            <div style="
                                                margin-top:5px;
                                                font-size:13px;
                                                line-height:1.5;
                                                color:#6b7280;
                                            ">
                                                Ihre zuverlässige Lösung für
                                                Reinigung, Räumung und
                                                Gartenarbeiten
                                            </div>

                                        </td>

                                        <td
                                            align="right"
                                            valign="top"
                                        >

                                            <div style="
                                                font-size:11px;
                                                letter-spacing:1px;
                                                color:#9ca3af;
                                            ">
                                                OFFERTE
                                            </div>

                                            <div style="
                                                margin-top:5px;
                                                font-size:15px;
                                                font-weight:bold;
                                                color:#111827;
                                            ">
                                                {{quote.QuoteNumber}}
                                            </div>

                                        </td>

                                    </tr>
                                </table>

                            </td>
                        </tr>


                        <!-- =====================================================
                             CONTENT
                             ===================================================== -->

                        <tr>
                            <td style="
                                padding:36px;
                            ">

                                <div style="
                                    font-size:14px;
                                    color:#6b7280;
                                    margin-bottom:8px;
                                ">
                                    Guten Tag {{firstName}}
                                </div>


                                <h1 style="
                                    margin:0 0 18px 0;
                                    font-size:28px;
                                    line-height:1.25;
                                    color:#111827;
                                ">
                                    Ihre Offerte
                                </h1>


                                <p style="
                                    margin:0;
                                    font-size:15px;
                                    line-height:1.7;
                                    color:#4b5563;
                                ">
                                    Vielen Dank für Ihre Anfrage und Ihr
                                    Interesse an unseren Dienstleistungen.
                                </p>


                                <p style="
                                    margin:14px 0 0 0;
                                    font-size:15px;
                                    line-height:1.7;
                                    color:#4b5563;
                                ">
                                    Im Anhang dieser E-Mail finden Sie Ihre
                                    persönliche Offerte als PDF-Dokument.
                                </p>


                                <!-- =================================================
                                     QUOTE SUMMARY
                                     ================================================= -->

                                <table
                                    width="100%"
                                    cellpadding="0"
                                    cellspacing="0"
                                    border="0"
                                    style="
                                        margin-top:28px;
                                        background:#f9fafb;
                                        border:1px solid #e5e7eb;
                                        border-radius:8px;
                                    "
                                >
                                    <tr>
                                        <td style="
                                            padding:20px 22px;
                                        ">

                                            <table
                                                width="100%"
                                                cellpadding="0"
                                                cellspacing="0"
                                                border="0"
                                            >

                                                <!-- Quote number -->

                                                <tr>

                                                    <td style="
                                                        padding-bottom:14px;
                                                        font-size:13px;
                                                        color:#6b7280;
                                                    ">
                                                        Offerten-Nr.
                                                    </td>

                                                    <td
                                                        align="right"
                                                        style="
                                                            padding-bottom:14px;
                                                            font-size:14px;
                                                            font-weight:bold;
                                                            color:#111827;
                                                        "
                                                    >
                                                        {{quote.QuoteNumber}}
                                                    </td>

                                                </tr>


                                                <!-- Valid until -->

                                                <tr>

                                                    <td style="
                                                        padding-bottom:14px;
                                                        font-size:13px;
                                                        color:#6b7280;
                                                    ">
                                                        Gültig bis
                                                    </td>

                                                    <td
                                                        align="right"
                                                        style="
                                                            padding-bottom:14px;
                                                            font-size:14px;
                                                            color:#111827;
                                                        "
                                                    >
                                                        {{validUntil}}
                                                    </td>

                                                </tr>


                                                <!-- Total -->

                                                <tr>

                                                    <td
                                                        colspan="2"
                                                        style="
                                                            border-top:1px solid #e5e7eb;
                                                            padding-top:16px;
                                                        "
                                                    >

                                                        <table
                                                            width="100%"
                                                            cellpadding="0"
                                                            cellspacing="0"
                                                            border="0"
                                                        >
                                                            <tr>

                                                                <td style="
                                                                    font-size:14px;
                                                                    font-weight:bold;
                                                                    color:#374151;
                                                                ">
                                                                    Gesamtbetrag
                                                                </td>

                                                                <td
                                                                    align="right"
                                                                    style="
                                                                        font-size:22px;
                                                                        font-weight:bold;
                                                                        color:#111827;
                                                                    "
                                                                >
                                                                    CHF {{total}}
                                                                </td>

                                                            </tr>
                                                        </table>

                                                    </td>

                                                </tr>

                                            </table>

                                        </td>
                                    </tr>
                                </table>


                                <!-- =================================================
                                     PDF ATTACHMENT
                                     ================================================= -->

                                <table
                                    width="100%"
                                    cellpadding="0"
                                    cellspacing="0"
                                    border="0"
                                    style="
                                        margin-top:24px;
                                        border:1px solid #d1d5db;
                                        border-radius:8px;
                                        background:#ffffff;
                                    "
                                >
                                    <tr>

                                        <td
                                            width="56"
                                            valign="middle"
                                            style="
                                                padding:18px 8px 18px 18px;
                                                font-size:28px;
                                            "
                                        >
                                            📄
                                        </td>

                                        <td
                                            valign="middle"
                                            style="
                                                padding:18px 18px 18px 8px;
                                            "
                                        >

                                            <div style="
                                                font-size:14px;
                                                font-weight:bold;
                                                color:#111827;
                                            ">
                                                Ihre Offerte als PDF
                                            </div>

                                            <div style="
                                                margin-top:5px;
                                                font-size:13px;
                                                color:#4b5563;
                                            ">
                                                {{quote.QuoteNumber}}.pdf
                                            </div>

                                            <div style="
                                                margin-top:5px;
                                                font-size:12px;
                                                color:#9ca3af;
                                            ">
                                                PDF-Datei im Anhang
                                                dieser E-Mail
                                            </div>

                                        </td>

                                    </tr>
                                </table>


                                <!-- =================================================
                                     CLOSING
                                     ================================================= -->

                                <p style="
                                    margin:28px 0 0 0;
                                    font-size:15px;
                                    line-height:1.7;
                                    color:#4b5563;
                                ">
                                    Bei Fragen zur Offerte oder zu unseren
                                    Dienstleistungen stehen wir Ihnen
                                    gerne zur Verfügung.
                                </p>


                                <p style="
                                    margin:28px 0 0 0;
                                    font-size:15px;
                                    line-height:1.6;
                                    color:#374151;
                                ">
                                    Freundliche Grüsse<br>
                                    <strong>Leu Gebäudeservice</strong>
                                </p>

                            </td>
                        </tr>


                        <!-- =====================================================
                             FOOTER
                             ===================================================== -->

                        <tr>
                            <td style="
                                padding:22px 36px;
                                background:#f9fafb;
                                border-top:1px solid #e5e7eb;
                            ">

                                <div style="
                                    font-size:12px;
                                    line-height:1.6;
                                    color:#9ca3af;
                                ">
                                    Diese E-Mail wurde im Zusammenhang mit
                                    Ihrer Offerte
                                    {{quote.QuoteNumber}}
                                    automatisch erstellt.
                                </div>

                            </td>
                        </tr>

                    </table>

                </td>
            </tr>
        </table>

        </body>
        </html>
        """;
    }

    private static string HtmlEncode(string? value)
    {
        return System.Net.WebUtility.HtmlEncode(
            value ?? string.Empty);
    }
}