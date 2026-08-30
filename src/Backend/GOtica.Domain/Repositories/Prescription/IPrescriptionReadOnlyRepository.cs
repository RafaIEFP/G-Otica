namespace GOtica.Domain.Repositories.Prescription;

public interface IPrescriptionReadOnlyRepository
{
    Task<Entities.Prescription?> GetById(Guid prescriptionId, Guid clientId, Guid opticalStoreId);
}
