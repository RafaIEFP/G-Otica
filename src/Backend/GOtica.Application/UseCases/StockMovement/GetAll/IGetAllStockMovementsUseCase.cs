using GOtica.Communication.Requests.StockMovement;
using GOtica.Communication.Response;
using GOtica.Communication.Response.StockMovement;

namespace GOtica.Application.UseCases.StockMovement.GetAll;

public interface IGetAllStockMovementsUseCase
{
    Task<ResponsePaged<ResponseStockMovement>> Execute(Guid opticalStoreId, Guid productId, RequestGetStockMovements request);
}
