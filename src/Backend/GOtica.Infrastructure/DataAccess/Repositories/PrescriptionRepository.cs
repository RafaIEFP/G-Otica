using GOtica.Domain.Entities;
using GOtica.Domain.Repositories.Prescription;
using Microsoft.EntityFrameworkCore;

namespace GOtica.Infrastructure.DataAccess.Repositories;

internal sealed class PrescriptionRepository(GOticaDbContext dbContext) : IPrescriptionWriteOnlyRepository, IPrescriptionReadOnlyRepository
{
    public async Task Add(Prescription prescription)
    {
        await dbContext.Prescriptions.AddAsync(prescription);
    }

    public async Task<Prescription?> GetById(Guid prescriptionId, Guid clientId, Guid opticalStoreId)
    {
        return await dbContext.Prescriptions
            .AsNoTracking()
            .FirstOrDefaultAsync(prescription =>
                prescription.Id == prescriptionId &&
                prescription.ClientId == clientId &&
                prescription.Client.OpticalStoreId == opticalStoreId);
    }
}
