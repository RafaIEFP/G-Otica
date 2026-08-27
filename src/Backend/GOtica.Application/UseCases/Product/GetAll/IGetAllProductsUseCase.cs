using GOtica.Communication.Requests.Product;
using GOtica.Communication.Response;
using GOtica.Communication.Response.Product;

namespace GOtica.Application.UseCases.Product.GetAll;

public interface IGetAllProductsUseCase
{
    Task<ResponsePaged<ResponseGetAllProduct>> Execute(Guid opticalStoreId, RequestGetAllProducts request);
}
