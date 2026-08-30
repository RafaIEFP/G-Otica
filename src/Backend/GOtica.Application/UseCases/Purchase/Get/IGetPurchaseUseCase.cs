using GOtica.Communication.Response.Purchase;

namespace GOtica.Application.UseCases.Purchase.Get;

public interface IGetPurchaseUseCase
{
    Task<ResponseGetPurchase> Execute(Guid opticalStoreId, Guid purchaseId);
}
