using GOtica.Communication.Response.Sale;

namespace GOtica.Application.UseCases.Sale.Get;

public interface IGetSaleUseCase
{
    Task<ResponseGetSale> Execute(Guid saleId, Guid opticalStoreId);
}
