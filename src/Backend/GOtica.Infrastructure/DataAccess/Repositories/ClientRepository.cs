using GOtica.Domain.Dtos;
using GOtica.Domain.Entities;
using GOtica.Domain.Repositories;
using GOtica.Domain.Repositories.Client;
using Microsoft.EntityFrameworkCore;

namespace GOtica.Infrastructure.DataAccess.Repositories;

internal sealed class ClientRepository(GOticaDbContext dbContext) : IClientWriteOnlyRepository, IClientReadOnlyRepository, IClientUpdateOnlyRepository
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

    public async Task<PagedResult<ClientDto>> GetAll(Guid opticalStoreId, int page, int pageSize, bool? isActive)
    {
        var query = dbContext.Clients
            .AsNoTracking()
            .Where(client => client.OpticalStoreId == opticalStoreId);

        if (isActive.HasValue)
        {
            query = query.Where(client =>
                client.IsActive == isActive.Value);
        }

        var totalCount = await query.CountAsync();

        var clients = await query
            .OrderBy(client => client.Name)
            .Paged(page, pageSize)
            .Select(client => new ClientDto
            {
                Id = client.Id,
                Name = client.Name,
                PhoneNumber = client.PhoneNumber,
                Email = client.Email,
                DateOfBirth = client.DateOfBirth,
                IsActive = client.IsActive
            })
            .ToListAsync();

        return new PagedResult<ClientDto>
        {
            Items = clients,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<Client?> GetActiveInOpticalStore(Guid clientId, Guid opticalStoreId)
    {
        return await dbContext.Clients
            .FirstOrDefaultAsync(c => c.Id == clientId && c.OpticalStoreId == opticalStoreId && c.IsActive);
    }
}
