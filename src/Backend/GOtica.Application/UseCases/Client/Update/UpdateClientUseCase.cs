using GOtica.Communication.Requests.Client;
using GOtica.Domain.Repositories;
using GOtica.Domain.Repositories.Client;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;
using Mapster;

namespace GOtica.Application.UseCases.Client.Update;

public class UpdateClientUseCase : IUpdateClientUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClientUpdateOnlyRepository _clientUpdateOnlyRepository;
    public UpdateClientUseCase(IUnitOfWork unitOfWork, IClientUpdateOnlyRepository clientUpdateOnlyRepository)
    {
        _clientUpdateOnlyRepository = clientUpdateOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid opticalStoreId, Guid clientId, RequestUpdateClient request)
    {
        Validate(request);

        var client = await _clientUpdateOnlyRepository.GetActiveInOpticalStore(clientId, opticalStoreId)
            ??
            throw new NotFoundException(ResourceMessagesException.CLIENT_NOT_FOUND);

        request.Adapt(client);

        await _unitOfWork.Commit();
    }

    private static void Validate(RequestUpdateClient request)
    {
        var result = new UpdateClientValidator().Validate(request);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
