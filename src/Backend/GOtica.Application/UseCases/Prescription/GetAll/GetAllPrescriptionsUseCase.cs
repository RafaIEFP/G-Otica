using GOtica.Communication.Requests.Prescription;
using GOtica.Communication.Response;
using GOtica.Communication.Response.Prescription;
using GOtica.Domain.Repositories.Client;
using GOtica.Domain.Repositories.Prescription;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;
using Mapster;

namespace GOtica.Application.UseCases.Prescription.GetAll;

public class GetAllPrescriptionsUseCase : IGetAllPrescriptionsUseCase
{
    private readonly IClientReadOnlyRepository _clientReadOnlyRepository;
    private readonly IPrescriptionReadOnlyRepository _prescriptionReadOnlyRepository;
    public GetAllPrescriptionsUseCase(
        IClientReadOnlyRepository clientReadOnlyRepository,
        IPrescriptionReadOnlyRepository prescriptionReadOnlyRepository)
    {
        _clientReadOnlyRepository = clientReadOnlyRepository;
        _prescriptionReadOnlyRepository = prescriptionReadOnlyRepository;
    }

    public async Task<ResponsePaged<ResponseGetAllPrescription>> Execute(Guid opticalStoreId, Guid clientId, RequestGetAllPrescriptions request)
    {
        Validate(request);

        var clientExists = await _clientReadOnlyRepository.Exist(clientId, opticalStoreId);

        if (!clientExists)
            throw new NotFoundException(ResourceMessagesException.CLIENT_NOT_FOUND);

        var result = await _prescriptionReadOnlyRepository.GetAll(clientId, opticalStoreId, request.Page, request.PageSize);

        return new ResponsePaged<ResponseGetAllPrescription>
        {
            Items = result.Items
                .Adapt<IReadOnlyCollection<ResponseGetAllPrescription>>(),

            Page = result.Page,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount,
            TotalPages = result.TotalPages
        };
    }

    private static void Validate(RequestGetAllPrescriptions request)
    {
        var result = new GetAllPrescriptionsValidator().Validate(request);

        if (!result.IsValid)
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
    }
}
