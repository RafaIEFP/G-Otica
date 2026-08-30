using GOtica.Communication.Requests.Purchase;
using GOtica.Communication.Response.Purchase;

namespace GOtica.Application.UseCases.Purchase.Register;

public interface IRegisterPurchaseUseCase
{
    Task<ResponseRegisterPurchase> Execute(Guid opticalStoreId, RequestRegisterPurchase request);
}
