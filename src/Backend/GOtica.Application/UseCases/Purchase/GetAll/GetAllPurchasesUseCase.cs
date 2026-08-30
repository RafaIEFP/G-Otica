using GOtica.Communication.Requests.Purchase;
using GOtica.Communication.Response;
using GOtica.Communication.Response.Purchase;
using GOtica.Domain.Repositories.Purchase;
using GOtica.Exceptions.ExceptionsBase;
using Mapster;

namespace GOtica.Application.UseCases.Purchase.GetAll;

public class GetAllPurchasesUseCase : IGetAllPurchasesUseCase
{
    private readonly IPurchaseReadOnlyRepository _purchaseReadOnlyRepository;
    public GetAllPurchasesUseCase(IPurchaseReadOnlyRepository purchaseReadOnlyRepository)
    {
        _purchaseReadOnlyRepository = purchaseReadOnlyRepository;
    }

    public async Task<ResponsePaged<ResponseGetAllPurchase>> Execute(Guid opticalStoreId, RequestGetAllPurchases request)
    {
        Validate(request);

        var result = await _purchaseReadOnlyRepository.GetAll(opticalStoreId, request.Page, request.PageSize);

        return new ResponsePaged<ResponseGetAllPurchase>
        {
            Items = result.Items.Adapt<IReadOnlyCollection<ResponseGetAllPurchase>>(),

            Page = result.Page,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount,
            TotalPages = result.TotalPages
        };
    }

    private static void Validate(RequestGetAllPurchases request)
    {
        var result = new GetAllPurchasesValidator().Validate(request);

        if (!result.IsValid)
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
    }
}
