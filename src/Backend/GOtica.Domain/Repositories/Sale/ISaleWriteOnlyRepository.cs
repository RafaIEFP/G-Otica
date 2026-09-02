namespace GOtica.Domain.Repositories.Sale;

public interface ISaleWriteOnlyRepository
{
    Task Add(Entities.Sale sale);
}
