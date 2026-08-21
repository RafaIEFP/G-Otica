using MimeKit;

namespace GOtica.Infrastructure.Services.Email;

internal sealed class EmailBuilder
{
    private readonly MimeMessage _message;
    public EmailBuilder()
    {
        _message = new MimeMessage();
    }

    public EmailBuilder WithSender(string senderName, string senderEmail)
    {
        _message.From.Add(new MailboxAddress(senderName, senderEmail));

        return this;
    }

    public EmailBuilder WithDestination(string to)
    {
        _message.To.Add(MailboxAddress.Parse(to));

        return this;
    }

    public EmailBuilder WithSubject(string subject)
    {
        _message.Subject = subject;

        return this;
    }

    public EmailBuilder WithBody(MimeEntity body)
    {
        _message.Body = body;

        return this;
    }

    public MimeMessage Build() => _message;

}
