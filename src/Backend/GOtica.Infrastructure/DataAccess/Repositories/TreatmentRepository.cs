using GOtica.Domain.Dtos;
using GOtica.Domain.Entities;
using GOtica.Domain.Repositories;
using GOtica.Domain.Repositories.Treatment;
using Microsoft.EntityFrameworkCore;

namespace GOtica.Infrastructure.DataAccess.Repositories;

internal sealed class TreatmentRepository(GOticaDbContext dbContext) : ITreatmentReadOnlyRepository, ITreatmentWriteOnlyRepository, ITreatmentUpdateOnlyRepository
{
    public async Task Add(Treatment treatment)
    {
        await dbContext.Treatments.AddAsync(treatment);
    }

    public async Task<bool> Deactivate(Guid treatmentId, Guid opticalStoreId)
    {
        var affectedRows = await dbContext.Treatments
            .Where(treatment =>
                treatment.Id == treatmentId &&
                treatment.OpticalStoreId == opticalStoreId &&
                treatment.IsActive)
            .ExecuteUpdateAsync(
                setter => setter.SetProperty(
                    treatment => treatment.IsActive,
                    false));

        return affectedRows > 0;
    }

    public async Task<Treatment?> GetActiveInOpticalStore(Guid treatmentId, Guid opticalStoreId)
    {
        return await dbContext.Treatments
            .FirstOrDefaultAsync(treatment =>
                treatment.Id == treatmentId &&
                treatment.OpticalStoreId == opticalStoreId &&
                treatment.IsActive);
    }

    public async Task<IReadOnlyCollection<Treatment>> GetActivesByIds(IReadOnlyCollection<Guid> treatmentIds, Guid opticalStoreId)
    {
        return await dbContext.Treatments
            .AsNoTracking()
            .Where(treatment =>
                treatmentIds.Contains(treatment.Id) &&
                treatment.OpticalStoreId == opticalStoreId &&
                treatment.IsActive)
            .ToListAsync();
    }

    public async Task<PagedResult<TreatmentDto>> GetAll(Guid opticalStoreId, int page, int pageSize, bool? isActive)
    {
        var query = dbContext.Treatments.AsNoTracking().Where(treatment => treatment.OpticalStoreId == opticalStoreId);

        if (isActive.HasValue)
            query = query.Where(treatment => treatment.IsActive == isActive.Value);

        var totalCount = await query.CountAsync();

        var treatments = await query
            .OrderBy(treatment => treatment.Name)
            .ThenBy(treatment => treatment.Id)
            .Paged(page, pageSize)
            .Select(treatment => new TreatmentDto
            {
                Id = treatment.Id,
                Name = treatment.Name,
                BasePrice = treatment.BasePrice,
                IsActive = treatment.IsActive
            })
            .ToListAsync();

        return new PagedResult<TreatmentDto>
        {
            Items = treatments,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<Treatment?> GetById(Guid treatmentId, Guid opticalStoreId)
    {
        return await dbContext.Treatments
            .AsNoTracking()
            .FirstOrDefaultAsync(treatment =>
                treatment.Id == treatmentId &&
                treatment.OpticalStoreId == opticalStoreId);
    }

    public async Task<bool> Reactivate(Guid treatmentId, Guid opticalStoreId)
    {
        var affectedRows = await dbContext.Treatments
            .Where(treatment =>
                treatment.Id == treatmentId &&
                treatment.OpticalStoreId == opticalStoreId &&
                !treatment.IsActive)
            .ExecuteUpdateAsync(
                setter => setter.SetProperty(
                    treatment => treatment.IsActive,
                    true));

        return affectedRows > 0;
    }

    public async Task<bool> TreatmentAlreadyAtOpticalStore(string name, Guid opticalStoreId, Guid? exceptTreatmentId = null)
    {
        var normalizedName = name.ToUpperInvariant();

        return await dbContext.Treatments
            .AsNoTracking()
            .AnyAsync(treatment =>
                treatment.OpticalStoreId == opticalStoreId &&
                treatment.Id != exceptTreatmentId &&
                treatment.Name.ToUpper() == normalizedName);
    }
}
