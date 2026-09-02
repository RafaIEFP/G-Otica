using GOtica.Communication.Requests.Sale;
using GOtica.Communication.Response.Sale;

namespace GOtica.Application.UseCases.Sale.Register;

public interface IRegisterSaleUseCase
{
    Task<ResponseRegisterSale> Execute(Guid opticalStoreId, RequestRegisterSale request);
}
