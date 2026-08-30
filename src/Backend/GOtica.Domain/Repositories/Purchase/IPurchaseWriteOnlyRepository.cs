namespace GOtica.Domain.Repositories.Purchase;

public interface IPurchaseWriteOnlyRepository
{
    Task Add(Entities.Purchase purchase);
}
