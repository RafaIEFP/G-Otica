namespace GOtica.Domain.Repositories.OpticalStore;

public interface IOpticalStoreWriteOnlyRepository
{
    Task Add(Entities.OpticalStore opticalStore);
}
