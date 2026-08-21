using GOtica.Communication.Requests;

namespace GOtica.Application.UseCases.UserOpticalStore.Invite;

public interface ICreateInviteUseCase
{
    Task Execute(Guid opticalStoreId, RequestInvite request);
}
