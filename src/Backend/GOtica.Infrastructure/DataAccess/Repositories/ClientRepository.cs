using GOtica.Domain.Entities;
using GOtica.Domain.Repositories.Client;

namespace GOtica.Infrastructure.DataAccess.Repositories;

internal sealed class ClientRepository(GOticaDbContext dbContext) : IClientWriteOnlyRepository
{
    public async Task Add(Client client)
    {
        await dbContext.Clients.AddAsync(client);
    }
}
