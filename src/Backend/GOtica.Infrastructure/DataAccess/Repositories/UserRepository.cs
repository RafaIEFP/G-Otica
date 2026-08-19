using GOtica.Domain.Entities;
using GOtica.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace GOtica.Infrastructure.DataAccess.Repositories;

internal sealed class UserRepository(GOticaDbContext dbContext) : IUserReadOnlyRepository, IUserWriteOnlyRepository, IUserUpdateOnlyRepository
{
    public async Task Add(User user) => await dbContext.Users.AddAsync(user);

    public async Task<bool> ExistUserWithEmail(string email) 
        => await dbContext.Users.AnyAsync(user => user.Email.Equals(email));

    public async Task<User?> GetUserByEmail(string email) 
        => await dbContext.Users.FirstOrDefaultAsync(user => user.Email.Equals(email) && user.IsActive);

    async Task<User?> IUserReadOnlyRepository.GetUserById(Guid id)
         => await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);

    async Task<User> IUserUpdateOnlyRepository.GetUserById(Guid id)
        => await dbContext.Users.FirstAsync(u => u.Id == id);

    public void Update(User user)
        => dbContext.Users.Update(user);

    public async Task DeleteAccount(Guid userId)
        => await dbContext.Users.Where(u => u.Id == userId).ExecuteDeleteAsync();

    public async Task<bool> ExistsActiveUser(Guid id)
        => await dbContext.Users.AnyAsync(u => u.Id == id && u.IsActive);

    public async Task DeactivateAccount(Guid userId)
    => await dbContext.Users
        .Where(u => u.Id == userId)
        .ExecuteUpdateAsync(
            setter => setter.SetProperty(u => u.IsActive, false)
        );
}
