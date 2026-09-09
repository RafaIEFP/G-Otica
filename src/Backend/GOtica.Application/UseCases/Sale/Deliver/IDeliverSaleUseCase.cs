namespace GOtica.Application.UseCases.Sale.Deliver;

public interface IDeliverSaleUseCase
{
    Task Execute(Guid opticalStoreId, Guid saleId);
}
