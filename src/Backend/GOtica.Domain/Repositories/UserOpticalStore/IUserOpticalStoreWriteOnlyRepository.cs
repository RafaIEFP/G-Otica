namespace GOtica.Domain.Repositories.UserOpticalStore;

public interface IUserOpticalStoreWriteOnlyRepository
{
    Task Add(Entities.UserOpticalStore userOpticalStore);
}
