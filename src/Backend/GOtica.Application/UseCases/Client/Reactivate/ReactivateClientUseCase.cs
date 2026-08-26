using GOtica.Domain.Repositories.Client;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;

namespace GOtica.Application.UseCases.Client.Reactivate;

public class ReactivateClientUseCase : IReactivateClientUseCase
{
    private readonly IClientUpdateOnlyRepository _clientUpdateOnlyRepository;
    public ReactivateClientUseCase(IClientUpdateOnlyRepository clientUpdateOnlyRepository)
    {
        _clientUpdateOnlyRepository = clientUpdateOnlyRepository;
    }

    public async Task Execute(Guid opticalStoreId, Guid clientId)
    {
        var reactivated = await _clientUpdateOnlyRepository.Reactivate(clientId, opticalStoreId);

        if (!reactivated)
            throw new NotFoundException(ResourceMessagesException.CLIENT_NOT_FOUND);
    }
}
