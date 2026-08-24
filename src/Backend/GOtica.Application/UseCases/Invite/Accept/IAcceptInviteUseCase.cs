using GOtica.Communication.Requests.Invite;

namespace GOtica.Application.UseCases.Invite.Accept;

public interface IAcceptInviteUseCase
{
    Task Execute(RequestAcceptInvite request);
}
