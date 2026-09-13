using GOtica.Domain.Entities;
using GOtica.Domain.Repositories.Treatment;
using Microsoft.EntityFrameworkCore;

namespace GOtica.Infrastructure.DataAccess.Repositories;

internal sealed class TreatmentRepository(GOticaDbContext dbContext) : ITreatmentReadOnlyRepository, ITreatmentWriteOnlyRepository
{
    public async Task Add(Treatment treatment)
    {
        await dbContext.Treatments.AddAsync(treatment);
    }

    public async Task<Treatment?> GetById(Guid treatmentId, Guid opticalStoreId)
    {
        return await dbContext.Treatments
            .AsNoTracking()
            .FirstOrDefaultAsync(treatment =>
                treatment.Id == treatmentId &&
                treatment.OpticalStoreId == opticalStoreId);
    }

    public async Task<bool> TreatmentAlreadyAtOpticalStore(string name, Guid opticalStoreId)
    {
        var normalizedName = name.ToUpperInvariant();

        return await dbContext.Treatments
            .AsNoTracking()
            .AnyAsync(treatment =>
                treatment.OpticalStoreId == opticalStoreId &&
                treatment.Name.ToUpper() == normalizedName);
    }
}
