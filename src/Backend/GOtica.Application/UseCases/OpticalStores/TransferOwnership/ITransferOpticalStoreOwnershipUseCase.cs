namespace GOtica.Application.UseCases.OpticalStores.TransferOwnership;

public interface ITransferOpticalStoreOwnershipUseCase
{
    Task Execute(Guid newOwnerUserId, long opticalId);
}