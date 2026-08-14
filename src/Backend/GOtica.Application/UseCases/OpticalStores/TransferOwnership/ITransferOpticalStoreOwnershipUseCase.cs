namespace GOtica.Application.UseCases.OpticalStores.TransferOwnership;

public interface ITransferOpticalStoreOwnershipUseCase
{
    Task Execute(Guid newOwnerUserId, Guid opticalId);
}