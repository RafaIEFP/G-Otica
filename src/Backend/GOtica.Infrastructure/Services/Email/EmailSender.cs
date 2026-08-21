using GOtica.Domain.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;

namespace GOtica.Infrastructure.Services.Email;

internal sealed class EmailSender(
    IOptions<EmailSettings> options) : IEmailSender
{
    private readonly EmailSettings _settings = options.Value;

    public async Task Send(string ownerName, string to, string inviteLink)
    {
        var email = new EmailBuilder()
            .WithSender(_settings.SenderName, _settings.SenderEmail)
            .WithDestination(to)
            .WithSubject("You have been invited to G-Otica")
            .WithBody(InviteEmailTemplate.Generate(ownerName, inviteLink))
            .Build();

        using var smtpClient = new SmtpClient();

        await smtpClient.ConnectAsync(
            _settings.Server,
            _settings.Port,
            SecureSocketOptions.StartTls);

        await smtpClient.AuthenticateAsync(
            _settings.Login,
            _settings.Password);

        await smtpClient.SendAsync(email);

        await smtpClient.DisconnectAsync(true);
    }
}
