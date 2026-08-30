namespace GOtica.Domain.Repositories.Prescription;

public interface IPrescriptionWriteOnlyRepository
{
    Task Add(Entities.Prescription prescription);
}