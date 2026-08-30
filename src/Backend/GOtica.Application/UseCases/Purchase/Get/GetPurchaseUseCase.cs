using GOtica.Communication.Response.Purchase;
using GOtica.Domain.Repositories.Purchase;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;
using Mapster;

namespace GOtica.Application.UseCases.Purchase.Get;

public class GetPurchaseUseCase : IGetPurchaseUseCase
{
    private readonly IPurchaseReadOnlyRepository _purchaseReadOnlyRepository;
    public GetPurchaseUseCase(
        IPurchaseReadOnlyRepository purchaseReadOnlyRepository)
    {
        _purchaseReadOnlyRepository = purchaseReadOnlyRepository;
    }

    public async Task<ResponseGetPurchase> Execute(Guid opticalStoreId, Guid purchaseId)
    {
        var purchase = await _purchaseReadOnlyRepository.GetById(purchaseId, opticalStoreId)
            ??
            throw new NotFoundException(ResourceMessagesException.PURCHASE_NOT_FOUND);

        return purchase.Adapt<ResponseGetPurchase>();
    }
}
