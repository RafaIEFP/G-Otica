using GOtica.Domain.Dtos;

namespace GOtica.Domain.Repositories.Treatment;

public interface ITreatmentReadOnlyRepository
{
    Task<bool> TreatmentAlreadyAtOpticalStore(string name, Guid opticalStoreId, Guid? exceptTreatmentId = null);
    Task<Entities.Treatment?> GetById(Guid treatmentId, Guid opticalStoreId);
    Task<PagedResult<TreatmentDto>> GetAll(
        Guid opticalStoreId,
        int page,
        int pageSize,
        bool? isActive);

    Task<IReadOnlyCollection<Entities.Treatment>> GetActivesByIds(
        IReadOnlyCollection<Guid> treatmentIds,
        Guid opticalStoreId);
}
