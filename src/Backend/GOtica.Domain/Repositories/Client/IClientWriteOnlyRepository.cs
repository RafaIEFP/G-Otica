namespace GOtica.Domain.Repositories.Client;

public interface IClientWriteOnlyRepository
{
    Task Add(Entities.Client client);
}
