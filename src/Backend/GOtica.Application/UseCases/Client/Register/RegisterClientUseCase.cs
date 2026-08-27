using GOtica.Communication.Requests;
using GOtica.Communication.Requests.Client;
using GOtica.Communication.Response.Client;
using GOtica.Domain.Repositories;
using GOtica.Domain.Repositories.Client;
using GOtica.Exceptions.ExceptionsBase;
using Mapster;

namespace GOtica.Application.UseCases.Client.Register;

public class RegisterClientUseCase : IRegisterClientUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClientWriteOnlyRepository _clientWriteOnlyRepository;
    public RegisterClientUseCase(
        IUnitOfWork unitOfWork,
        IClientWriteOnlyRepository clientWriteOnlyRepository)
    {
        _unitOfWork = unitOfWork;
        _clientWriteOnlyRepository = clientWriteOnlyRepository;
    }

    public async Task<ResponseRegisterClient> Execute(Guid opticalStoreId, RequestRegisterClient request)
    {
        request = request.Normalize();

        Validate(request);

        var client = request.Adapt<Domain.Entities.Client>();

        client.OpticalStoreId = opticalStoreId;

        await _clientWriteOnlyRepository.Add(client);

        await _unitOfWork.Commit();

        return client.Adapt<ResponseRegisterClient>();
    }

    private void Validate(RequestRegisterClient request)
    {
        var result = new RegisterClientValidator().Validate(request);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
