using GOtica.Communication.Requests;

namespace GOtica.Application.UseCases.Invite.Accept;

public interface IAcceptInviteUseCase
{
    Task Execute(RequestAcceptInvite request);
}
