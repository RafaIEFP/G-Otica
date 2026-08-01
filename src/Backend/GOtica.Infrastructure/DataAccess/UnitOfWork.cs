using GOtica.Domain.Repositories;

namespace GOtica.Infrastructure.DataAccess;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly GOticaDbContext _dbContext;
    public UnitOfWork(GOticaDbContext dbContext) => _dbContext = dbContext; 

    public async Task Commit() => await _dbContext.SaveChangesAsync();  
}
