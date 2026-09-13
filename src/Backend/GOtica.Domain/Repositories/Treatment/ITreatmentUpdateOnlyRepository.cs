namespace GOtica.Domain.Repositories.Treatment;

public interface ITreatmentUpdateOnlyRepository
{
    Task<Entities.Treatment?> GetActiveInOpticalStore(Guid treatmentId, Guid opticalStoreId);
}
