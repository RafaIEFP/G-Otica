using GOtica.Communication.Response.Client;
using GOtica.Domain.Repositories.Client;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;
using Mapster;

namespace GOtica.Application.UseCases.Client.Get;

public class GetClientUseCase : IGetClientUseCase
{
    private readonly IClientReadOnlyRepository _clientReadOnlyRepository;
    public GetClientUseCase(IClientReadOnlyRepository clientReadOnlyRepository)
    {
        _clientReadOnlyRepository = clientReadOnlyRepository;
    }

    public async Task<ResponseGetClient> Execute(Guid opticalStoreId, Guid clientId)
    {
        var client = await _clientReadOnlyRepository.Get(clientId, opticalStoreId)
            ?? 
            throw new NotFoundException(ResourceMessagesException.CLIENT_NOT_FOUND);

        return client.Adapt<ResponseGetClient>();
    }
}
