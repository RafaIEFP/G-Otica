using GOtica.Domain.Repositories;

namespace GOtica.Infrastructure.DataAccess;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly GOticaDbContext _dbContext;
    public UnitOfWork(GOticaDbContext dbContext) => _dbContext = dbContext; 

    public async Task Commit() => await _dbContext.SaveChangesAsync();

    public async Task ExecuteInTransaction(Func<Task> action)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            await action();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
