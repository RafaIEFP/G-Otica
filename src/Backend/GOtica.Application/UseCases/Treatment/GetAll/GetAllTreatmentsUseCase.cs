using GOtica.Communication.Requests.Treatment;
using GOtica.Communication.Response;
using GOtica.Communication.Response.Treatment;
using GOtica.Domain.Repositories.Treatment;
using GOtica.Exceptions.ExceptionsBase;
using Mapster;

namespace GOtica.Application.UseCases.Treatment.GetAll;

public class GetAllTreatmentsUseCase : IGetAllTreatmentsUseCase
{
    private readonly ITreatmentReadOnlyRepository _treatmentReadOnlyRepository;
    public GetAllTreatmentsUseCase(
        ITreatmentReadOnlyRepository treatmentReadOnlyRepository)
    {
        _treatmentReadOnlyRepository = treatmentReadOnlyRepository;
    }

    public async Task<ResponsePaged<ResponseTreatment>> Execute(Guid opticalStoreId, RequestGetAllTreatments request)
    {
        Validate(request);

        var result = await _treatmentReadOnlyRepository.GetAll(
            opticalStoreId,
            request.Page,
            request.PageSize,
            request.IsActive);

        return new ResponsePaged<ResponseTreatment>
        {
            Items = result.Items.Adapt<IReadOnlyCollection<ResponseTreatment>>(),

            Page = result.Page,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount,
            TotalPages = result.TotalPages
        };
    }

    private static void Validate(RequestGetAllTreatments request)
    {
        var result = new GetAllTreatmentsValidator()
            .Validate(request);

        if (!result.IsValid)
            throw new ErrorOnValidationException([.. result.Errors.Select(error => error.ErrorMessage)]);
    }
}
