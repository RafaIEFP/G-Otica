namespace GOtica.Domain.Repositories.OpticalStore;

public interface IOpticalStoreUpdateOnlyRepository
{
    Task DeactivateOpticalStore(Guid opticalStoreId);
    Task<Entities.OpticalStore> GetById(Guid opticalStoreId);
    void Update(Entities.OpticalStore opticalStore);
}
