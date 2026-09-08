namespace GOtica.Application.UseCases.Sale.MarkAsReady;

public interface IMarkSaleAsReadyUseCase
{
    Task Execute(Guid opticalStoreId, Guid saleId);
}
