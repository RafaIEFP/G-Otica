namespace GOtica.Domain.Repositories.OpticalStore;

public interface IOpticalStoreReadOnlyRepository
{
    Task<bool> ExistsActiveOptical(Guid opticalId);
    Task<bool> ExistOpticalStoreRegistered(string taxNumber);
}
