using GOtica.Communication.Response;
using GOtica.Communication.Response.Sale;

namespace GOtica.Application.UseCases.Sale.GetAll;

public interface IGetAllSalesUseCase
{
    Task<ResponsePaged<ResponseGetAllSales>> Execute(Guid opticalStoreId, RequestGetAllSales request);
}
