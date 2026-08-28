using GOtica.Domain.Dtos;
using GOtica.Domain.Entities;
using GOtica.Domain.Repositories;
using GOtica.Domain.Repositories.Supplier;
using Microsoft.EntityFrameworkCore;

namespace GOtica.Infrastructure.DataAccess.Repositories;

internal sealed class SupplierRepository(GOticaDbContext dbContext) : ISupplierWriteOnlyRepository, ISupplierReadOnlyRepository
{
    public async Task Add(Supplier supplier)
    {
        await dbContext.Suppliers.AddAsync(supplier);
    }

    public async Task<PagedResult<SupplierDto>> GetAll(Guid opticalStoreId, int page, int pageSize, bool? isActive)
    {
        var query = dbContext.Suppliers
            .AsNoTracking()
            .Where(supplier => supplier.OpticalStoreId == opticalStoreId);

        if (isActive.HasValue)
        {
            query = query.Where(supplier => supplier.IsActive == isActive.Value);
        }

        var totalCount = await query.CountAsync();

        var suppliers = await query
            .OrderBy(supplier => supplier.Name)
            .ThenBy(supplier => supplier.Id)
            .Paged(page, pageSize)
            .Select(supplier => new SupplierDto
            {
                Id = supplier.Id,
                Name = supplier.Name,
                PhoneNumber = supplier.PhoneNumber,
                Email = supplier.Email,
                IsActive = supplier.IsActive
            })
            .ToListAsync();

        return new PagedResult<SupplierDto>
        {
            Items = suppliers,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<Supplier?> GetById(Guid supplierId, Guid opticalStoreId)
    {
        return await dbContext.Suppliers
        .AsNoTracking()
        .FirstOrDefaultAsync(supplier => supplier.Id == supplierId && supplier.OpticalStoreId == opticalStoreId);
    }
}
