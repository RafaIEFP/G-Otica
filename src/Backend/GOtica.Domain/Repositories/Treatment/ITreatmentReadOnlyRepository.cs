using GOtica.Domain.Dtos;

namespace GOtica.Domain.Repositories.Treatment;

public interface ITreatmentReadOnlyRepository
{
    Task<bool> TreatmentAlreadyAtOpticalStore(string name, Guid opticalStoreId);
    Task<Entities.Treatment?> GetById(Guid treatmentId, Guid opticalStoreId);
    Task<PagedResult<TreatmentDto>> GetAll(
        Guid opticalStoreId,
        int page,
        int pageSize,
        bool? isActive);
}
