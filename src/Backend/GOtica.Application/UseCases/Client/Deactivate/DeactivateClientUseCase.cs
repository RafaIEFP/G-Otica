using GOtica.Domain.Repositories.Client;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;

namespace GOtica.Application.UseCases.Client.Deactivate;

public class DeactivateClientUseCase : IDeactivateClientUseCase
{
    private readonly IClientUpdateOnlyRepository _clientUpdateOnlyRepository;
    public DeactivateClientUseCase(IClientUpdateOnlyRepository clientUpdateOnlyRepository)
    {
        _clientUpdateOnlyRepository = clientUpdateOnlyRepository;
    }

    public async Task Execute(Guid opticalStoreId, Guid clientId)
    {
        var deactivated = await _clientUpdateOnlyRepository.Deactivate(clientId, opticalStoreId);

        if (!deactivated)
            throw new NotFoundException(ResourceMessagesException.CLIENT_NOT_FOUND);
    }
}
