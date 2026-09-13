namespace GOtica.Domain.Repositories.Treatment;

public interface ITreatmentReadOnlyRepository
{
    Task<bool> TreatmentAlreadyAtOpticalStore(string name, Guid opticalStoreId);
}
