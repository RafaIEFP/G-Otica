namespace GOtica.Application.UseCases.Sale.StartProduction;

public interface IStartSaleProductionUseCase
{
    Task Execute(Guid opticalStoreId, Guid saleId);
}
