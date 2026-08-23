using GOtica.Communication.Response.Invite;

namespace GOtica.Application.UseCases.UserOpticalStore.Invite.Validade;

public interface IValidateInviteUseCase
{
    Task<ResponseValidateInvite> Execute(string token);
}
