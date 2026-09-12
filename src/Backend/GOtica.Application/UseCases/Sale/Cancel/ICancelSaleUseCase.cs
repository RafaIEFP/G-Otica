namespace GOtica.Application.UseCases.Sale.Cancel;

public interface ICancelSaleUseCase
{
    Task Execute(Guid opticalStoreId, Guid saleId);
}
