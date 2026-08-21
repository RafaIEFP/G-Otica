namespace GOtica.Domain.Services;

public interface IEmailSender
{
    Task Send(string ownerName, string to, string inviteLink);
}
