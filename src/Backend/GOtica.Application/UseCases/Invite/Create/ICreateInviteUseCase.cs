using GOtica.Communication.Requests;

namespace GOtica.Application.UseCases.UserOpticalStore.Invite.Create;

public interface ICreateInviteUseCase
{
    Task Execute(Guid opticalStoreId, RequestInvite request);
}
