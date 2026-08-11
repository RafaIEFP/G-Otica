using GOtica.Domain;
using GOtica.Domain.Entities;
using GOtica.Domain.Repositories.UserOpticalStore;
using Microsoft.EntityFrameworkCore;

namespace GOtica.Infrastructure.DataAccess.Repositories;

internal sealed class UserOpticalStoreRepository : IUserOpticalStoreReadOnlyRepository, IUserOpticalStoreUpdateOnlyRepository
{
    private readonly GOticaDbContext _dbContext;
    public UserOpticalStoreRepository(GOticaDbContext dbContext)
        => _dbContext = dbContext;

    public async Task<string> GetUserOpticalRole(Guid userId, long opticalId)
        => await _dbContext.UserOpticalStores
            .AsNoTracking()
            .Where(uos => uos.UserId == userId && uos.OpticalStoreId == opticalId)
            .Select(uos => uos.Role)
            .FirstAsync();

    public async Task<IReadOnlyCollection<UserOpticalStore>> GetUserOpticalStores(Guid userId)
    {
        return await _dbContext.UserOpticalStores
            .AsNoTracking()
            .Include(uos => uos.OpticalStore)
            .Where(uos => uos.UserId == userId)
            .ToListAsync();
    }

    public async Task UpdateUserRoleOpticalStore(Guid userId, long opticalId, string newRole)
    {
        await _dbContext.UserOpticalStores.Where(uos => uos.UserId == userId && uos.OpticalStoreId == opticalId)
           .ExecuteUpdateAsync(
               setter => setter.SetProperty(uos => uos.Role, newRole)
           );
    }

    public async Task<bool> UserBelongsToOptical(Guid userId, long opticalId)
    {
        return await _dbContext.UserOpticalStores
            .AnyAsync(uos => uos.UserId == userId && uos.OpticalStoreId == opticalId && uos.IsActive);
    }

    public async Task<bool> UserIsOwner(Guid userId)
        => await _dbContext.UserOpticalStores
            .AnyAsync(uos => uos.UserId == userId && uos.Role == Roles.OWNER);

    public async Task<bool> UserIsOwnerOfOpticalStore(Guid userId, long opticalId)
        => await _dbContext.UserOpticalStores
            .AnyAsync(
                uos => uos.UserId == userId && 
                uos.OpticalStoreId == opticalId && 
                uos.Role ==  Roles.OWNER
            );
}
