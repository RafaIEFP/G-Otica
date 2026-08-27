namespace GOtica.Domain.Repositories.Product;

public interface IProductWriteOnlyRepository
{
    Task Add(Entities.Product product);
}
