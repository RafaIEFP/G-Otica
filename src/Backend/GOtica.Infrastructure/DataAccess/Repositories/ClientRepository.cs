using GOtica.Domain.Entities;
using GOtica.Domain.Repositories.Client;
using Microsoft.EntityFrameworkCore;

namespace GOtica.Infrastructure.DataAccess.Repositories;

internal sealed class ClientRepository(GOticaDbContext dbContext) : IClientWriteOnlyRepository, IClientReadOnlyRepository
{
    public async Task Add(Client client)
    {
        await dbContext.Clients.AddAsync(client);
    }

    public async Task<Client?> Get(Guid clientId, Guid opticalStoreId)
    {
        return await dbContext.Clients
            .AsNoTracking()
            .FirstOrDefaultAsync(client => client.Id == clientId && client.OpticalStoreId == opticalStoreId);
    }
}
