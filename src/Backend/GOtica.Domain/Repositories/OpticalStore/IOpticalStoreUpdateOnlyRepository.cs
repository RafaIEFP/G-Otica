namespace GOtica.Domain.Repositories.OpticalStore;

public interface IOpticalStoreUpdateOnlyRepository
{
    Task DeactivateOpticalStore(Guid opticalStoreId);
}
