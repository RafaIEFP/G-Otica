using GOtica.Domain;
using GOtica.Domain.Entities;
using GOtica.Domain.Repositories.UserOpticalStore;
using Microsoft.EntityFrameworkCore;

namespace GOtica.Infrastructure.DataAccess.Repositories;

internal sealed class UserOpticalStoreRepository : IUserOpticalStoreReadOnlyRepository, IUserOpticalStoreUpdateOnlyRepository, IUserOpticalStoreWriteOnlyRepository
{
    private readonly GOticaDbContext _dbContext;
    public UserOpticalStoreRepository(GOticaDbContext dbContext)
        => _dbContext = dbContext;

    public async Task Add(UserOpticalStore userOpticalStore)
    {
        await _dbContext.UserOpticalStores.AddAsync(userOpticalStore);
    }

    public async Task DeactivateByOpticalStore(Guid opticalStoreId)
    {
        await _dbContext.UserOpticalStores
            .Where(uos => uos.OpticalStoreId == opticalStoreId && uos.IsActive)
            .ExecuteUpdateAsync(
                setter => setter.SetProperty(uos => uos.IsActive, false)
                );
    }

    public async Task DeactivateByUser(Guid userId)
        => await _dbContext.UserOpticalStores
            .Where(uos => uos.UserId == userId && uos.IsActive)
            .ExecuteUpdateAsync(
                setter => setter.SetProperty(uos => uos.IsActive, false)
                );

    public async Task<string> GetUserOpticalRole(Guid userId, Guid opticalId)
        => await _dbContext.UserOpticalStores
            .AsNoTracking()
            .Where(uos => uos.UserId == userId && uos.OpticalStoreId == opticalId)
            .Select(uos => uos.Role)
            .FirstAsync();

    public async Task<IReadOnlyCollection<UserOpticalStore>> GetUserOpticalStore(Guid userId)
    {
        return await _dbContext.UserOpticalStores
            .AsNoTracking()
            .Include(uos => uos.OpticalStore)
            .Where(uos => uos.UserId == userId)
            .ToListAsync();
    }

    public async Task UpdateUserRoleOpticalStoreAssociation(Guid userId, Guid opticalId, string newRole)
    {
        await _dbContext.UserOpticalStores.Where(uos => uos.UserId == userId && uos.OpticalStoreId == opticalId)
           .ExecuteUpdateAsync(
               setter => setter.SetProperty(uos => uos.Role, newRole)
           );
    }

    public async Task<bool> UserBelongsToOptical(Guid userId, Guid opticalId)
    {
        return await _dbContext.UserOpticalStores
            .AnyAsync(uos => uos.UserId == userId && uos.OpticalStoreId == opticalId && uos.IsActive);
    }

    public async Task<bool> UserIsOwner(Guid userId)
        => await _dbContext.UserOpticalStores
            .AnyAsync(uos => uos.UserId == userId && uos.Role == Roles.OWNER);

    public async Task<bool> UserIsOwnerOfOpticalStore(Guid userId, Guid opticalId)
        => await _dbContext.UserOpticalStores
            .AnyAsync(
                uos => uos.UserId == userId && 
                uos.OpticalStoreId == opticalId && 
                uos.Role ==  Roles.OWNER &&
                uos.IsActive
            );
}
