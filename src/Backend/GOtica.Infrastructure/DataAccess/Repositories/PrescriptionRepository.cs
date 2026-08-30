using GOtica.Domain.Dtos;
using GOtica.Domain.Entities;
using GOtica.Domain.Repositories;
using GOtica.Domain.Repositories.Prescription;
using Microsoft.EntityFrameworkCore;

namespace GOtica.Infrastructure.DataAccess.Repositories;

internal sealed class PrescriptionRepository(GOticaDbContext dbContext) : IPrescriptionWriteOnlyRepository, IPrescriptionReadOnlyRepository
{
    public async Task Add(Prescription prescription)
    {
        await dbContext.Prescriptions.AddAsync(prescription);
    }

    public async Task<PagedResult<PrescriptionListDto>> GetAll(Guid clientId, Guid opticalStoreId, int page, int pageSize)
    {
        var query = dbContext.Prescriptions
            .AsNoTracking()
            .Where(prescription =>
                prescription.ClientId == clientId &&
                prescription.Client.OpticalStoreId == opticalStoreId);

        var totalCount = await query.CountAsync();

        var prescriptions = await query
            .OrderByDescending(prescription => prescription.PrescriptionDate)
            .ThenByDescending(prescription => prescription.Id)
            .Paged(page, pageSize)
            .Select(prescription => new PrescriptionListDto
            {
                Id = prescription.Id,
                DoctorName = prescription.DoctorName,
                DoctorRegistration = prescription.DoctorRegistration,
                PrescriptionDate = prescription.PrescriptionDate,
                ExpirationDate = prescription.ExpirationDate
            })
            .ToListAsync();

        return new PagedResult<PrescriptionListDto>
        {
            Items = prescriptions,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
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
