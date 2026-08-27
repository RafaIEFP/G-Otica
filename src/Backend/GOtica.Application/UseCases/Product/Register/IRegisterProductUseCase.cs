using GOtica.Communication.Requests.Product;
using GOtica.Communication.Response.Product;

namespace GOtica.Application.UseCases.Product.Register;

public interface IRegisterProductUseCase
{
    Task<ResponseRegisterProduct> Execute(Guid opticalStoreId, RequestRegisterProduct request);
}
