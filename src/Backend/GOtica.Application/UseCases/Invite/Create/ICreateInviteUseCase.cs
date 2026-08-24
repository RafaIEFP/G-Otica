using GOtica.Communication.Requests.Invite;

namespace GOtica.Application.UseCases.UserOpticalStore.Invite.Create;

public interface ICreateInviteUseCase
{
    Task Execute(Guid opticalStoreId, RequestInvite request);
}
