using GOtica.Communication.Requests.Purchase;
using GOtica.Communication.Response;
using GOtica.Communication.Response.Purchase;

namespace GOtica.Application.UseCases.Purchase.GetAll;

public interface IGetAllPurchasesUseCase
{
    Task<ResponsePaged<ResponseGetAllPurchase>> Execute(Guid opticalStoreId, RequestGetAllPurchases request);
}
