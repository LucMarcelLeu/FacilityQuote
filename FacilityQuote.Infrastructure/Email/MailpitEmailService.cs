using System.Net;
using System.Net.Mail;
using FacilityQuote.Application.Email;
using Microsoft.Extensions.Options;

namespace FacilityQuote.Infrastructure.Email;

public class MailpitEmailService(
    IOptions<EmailOptions> options) : IEmailService
{
    private readonly EmailOptions _options = options.Value;

    public async Task SendAsync(
        string recipient,
        string subject,
        string htmlBody,
        byte[]? attachment = null,
        string? attachmentName = null,
        CancellationToken cancellationToken = default)
    {
        using var message = new MailMessage
        {
            From = new MailAddress(
                _options.FromAddress,
                _options.FromName),

            Subject = subject,

            Body = htmlBody,

            IsBodyHtml = true
        };

        message.To.Add(recipient);

        if (attachment is not null && !string.IsNullOrWhiteSpace(attachmentName))
        {
            var stream = new MemoryStream(attachment);

            message.Attachments.Add(
                new Attachment(
                    stream,
                    attachmentName,
                    "application/pdf"));
        }

        using var client = new SmtpClient(
            _options.SmtpHost,
            _options.SmtpPort)
        {
            EnableSsl = _options.UseSsl
        };

        await client.SendMailAsync(
            message,
            cancellationToken);
    }
}