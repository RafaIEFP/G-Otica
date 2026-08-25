using GOtica.Communication.Requests.Client;
using GOtica.Communication.Response;
using GOtica.Communication.Response.Client;
using GOtica.Domain.Repositories.Client;
using GOtica.Exceptions.ExceptionsBase;
using Mapster;

namespace GOtica.Application.UseCases.Client.GetAll;

public class GetAllClientsUseCase : IGetAllClientsUseCase
{
    private readonly IClientReadOnlyRepository _clientReadOnlyRepository;
    public GetAllClientsUseCase(IClientReadOnlyRepository clientReadOnlyRepository)
    {
        _clientReadOnlyRepository = clientReadOnlyRepository;
    }

    public async Task<ResponsePaged<ResponseGetAllClients>> Execute(Guid opticalStoreId, RequestGetAllClients request)
    {
        Validate(request);

        var result = await _clientReadOnlyRepository.GetAll(
            opticalStoreId,
            request.Page,
            request.PageSize,
            request.IsActive);

        return new ResponsePaged<ResponseGetAllClients>
        {
            Items = result.Items.Adapt<IReadOnlyCollection<ResponseGetAllClients>>(),

            Page = result.Page,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount,
            TotalPages = result.TotalPages
        };
    }

    private static void Validate(RequestGetAllClients request)
    {
        var result = new GetAllClientsValidator().Validate(request);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
