using GOtica.Domain.Repositories.Sale;

namespace GOtica.Application.UseCases.Sale.Deliver;

public class DeliverSaleUseCase : IDeliverSaleUseCase
{
    private readonly ISaleReadOnlyRepository _saleReadOnlyRepository;
    private readonly ISaleUpdateOnlyRepository _saleUpdateOnlyRepository;
    public DeliverSaleUseCase(
        ISaleReadOnlyRepository saleReadOnlyRepository,
        ISaleUpdateOnlyRepository saleUpdateOnlyRepository)
    {
        _saleReadOnlyRepository = saleReadOnlyRepository;
        _saleUpdateOnlyRepository = saleUpdateOnlyRepository;
    }

    public Task Execute(Guid opticalStoreId, Guid saleId)
    {
        throw new NotImplementedException();
    }
}
