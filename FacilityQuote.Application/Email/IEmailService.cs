namespace FacilityQuote.Application.Email;

public interface IEmailService
{
    Task SendAsync(
        string recipient,
        string subject,
        string htmlBody,
        byte[]? attachment = null,
        string? attachmentName = null,
        CancellationToken cancellationToken = default);
}