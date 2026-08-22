using GOtica.Communication.Response.UserOpticalStore;

namespace GOtica.Application.UseCases.UserOpticalStore.Invite.Validade;

public interface IValidateInviteUseCase
{
    Task<ResponseValidateInvite> Execute(string token);
}
