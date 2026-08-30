using GOtica.Domain.Dtos;

namespace GOtica.Domain.Repositories.Prescription;

public interface IPrescriptionReadOnlyRepository
{
    Task<Entities.Prescription?> GetById(Guid prescriptionId, Guid clientId, Guid opticalStoreId);
    Task<PagedResult<PrescriptionListDto>> GetAll(Guid clientId, Guid opticalStoreId, int page, int pageSize);
}
