namespace GOtica.Domain.Repositories.OpticalStore;

public interface IOpticalStoreReadOnlyRepository
{
    Task<bool> ExistsActiveOptical(long opticalId);
}
