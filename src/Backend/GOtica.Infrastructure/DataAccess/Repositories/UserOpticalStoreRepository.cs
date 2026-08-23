using GOtica.Domain;
using GOtica.Domain.Dtos;
using GOtica.Domain.Entities;
using GOtica.Domain.Repositories.UserOpticalStore;
using Microsoft.EntityFrameworkCore;

namespace GOtica.Infrastructure.DataAccess.Repositories;

internal sealed class UserOpticalStoreRepository(GOticaDbContext dbContext) : IUserOpticalStoreReadOnlyRepository, IUserOpticalStoreUpdateOnlyRepository, IUserOpticalStoreWriteOnlyRepository
{
    public async Task Add(UserOpticalStore userOpticalStore)
    {
        await dbContext.UserOpticalStores.AddAsync(userOpticalStore);
    }

    public async Task DeactivateByOpticalStore(Guid opticalStoreId)
    {
        await dbContext.UserOpticalStores
            .Where(uos => uos.OpticalStoreId == opticalStoreId && uos.IsActive)
            .ExecuteUpdateAsync(
                setter => setter.SetProperty(uos => uos.IsActive, false)
                );
    }

    public async Task DeactivateByUser(Guid userId)
        => await dbContext.UserOpticalStores
            .Where(uos => uos.UserId == userId && uos.IsActive)
            .ExecuteUpdateAsync(
                setter => setter.SetProperty(uos => uos.IsActive, false)
                );

    public async Task<IReadOnlyCollection<AllOpticalStoresWithRole>> GetAllOpticalStoresWithRole(Guid userId)
    {
        return await dbContext.UserOpticalStores
            .AsNoTracking()
            .Where(uos =>
                uos.UserId == userId &&
                uos.IsActive)
            .Select(uos => new AllOpticalStoresWithRole
            {
                Id = uos.OpticalStoreId,
                Name = uos.OpticalStore.Name,
                Role = uos.Role,
                EntranceDate = uos.EntranceDate,
                IsActive = uos.OpticalStore.IsActive
            })
            .ToListAsync();
    }

    public async Task<OpticalStoreWithRoleDTO?> GetOpticalStoreWithRole(Guid userId, Guid opticalId)
    {
        return await dbContext.UserOpticalStores
            .AsNoTracking()
            .Where(uos =>
                uos.UserId == userId &&
                uos.OpticalStoreId == opticalId &&
                uos.IsActive)
            .Select(uos => new OpticalStoreWithRoleDTO
            {
                Id = uos.OpticalStore.Id,
                Name = uos.OpticalStore.Name,
                Email = uos.OpticalStore.Email,
                PhoneNumber = uos.OpticalStore.PhoneNumber,
                TaxNumber = uos.OpticalStore.TaxNumber,
                EntranceDate = uos.EntranceDate,
                IsActive = uos.OpticalStore.IsActive,
                Role = uos.Role
            })
            .FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyCollection<UserOpticalStore>> GetAllUserOpticalStore(Guid userId)
    {
        return await dbContext.UserOpticalStores
            .AsNoTracking()
            .Include(uos => uos.OpticalStore)
            .Where(uos => uos.UserId == userId)
            .ToListAsync();
    }

    public async Task UpdateUserRoleOpticalStoreAssociation(Guid userId, Guid opticalId, string newRole)
    {
        await dbContext.UserOpticalStores.Where(uos => uos.UserId == userId && uos.OpticalStoreId == opticalId)
           .ExecuteUpdateAsync(
               setter => setter.SetProperty(uos => uos.Role, newRole)
           );
    }

    public async Task<bool> UserBelongsToOptical(Guid userId, Guid opticalId)
    {
        return await dbContext.UserOpticalStores
            .AnyAsync(uos => uos.UserId == userId && uos.OpticalStoreId == opticalId && uos.IsActive);
    }

    public async Task<bool> UserBelongsToOpticalByEmail(string email, Guid opticalId)
    {
        return await dbContext.UserOpticalStores
            .AnyAsync(uos => uos.User.Email == email && uos.OpticalStoreId == opticalId && uos.IsActive);
    }

    public async Task<bool> UserIsOwner(Guid userId)
        => await dbContext.UserOpticalStores
            .AnyAsync(uos => uos.UserId == userId && uos.Role == Roles.OWNER);

    public async Task<bool> UserIsOwnerOfOpticalStore(Guid userId, Guid opticalId)
        => await dbContext.UserOpticalStores
            .AnyAsync(
                uos => uos.UserId == userId && 
                uos.OpticalStoreId == opticalId && 
                uos.Role ==  Roles.OWNER &&
                uos.IsActive
            );

    public async Task<IReadOnlyCollection<OpticalStoreUsersDto>> GetAllOpticalStoreUsers(Guid opticalId)
    {
        return await dbContext.UserOpticalStores
            .AsNoTracking()
            .Where(uos => uos.OpticalStoreId == opticalId)
            .Select(uos => new OpticalStoreUsersDto
            {
                UserId = uos.UserId,
                Email = uos.User.Email,
                EntranceDate = uos.EntranceDate,
                IsActive = uos.IsActive,
                Name = uos.User.Name,
                Role = uos.Role
            })
            .ToListAsync();
            
    }

    public async Task<UserOpticalStore?> GetUserOpticalStore(Guid userId, Guid opticalId)
    {
        return await dbContext.UserOpticalStores
            .AsNoTracking()
            .FirstOrDefaultAsync(
                uos => uos.UserId == userId && 
                uos.OpticalStoreId == opticalId && 
                uos.IsActive
                );
    }

    public async Task DeactivateByUserAndOpticalStore(Guid userId, Guid opticalStoreId)
    {
        await dbContext.UserOpticalStores
        .Where(uos =>
            uos.UserId == userId &&
            uos.OpticalStoreId == opticalStoreId &&
            uos.IsActive)
        .ExecuteUpdateAsync(setter =>
            setter.SetProperty(uos => uos.IsActive, false));
    }
}
