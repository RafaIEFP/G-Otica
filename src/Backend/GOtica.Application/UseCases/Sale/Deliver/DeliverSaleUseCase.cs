using GOtica.Domain.Repositories.Sale;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;

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

    public async Task Execute(Guid opticalStoreId, Guid saleId)
    {
        var sale = await _saleReadOnlyRepository.GetDeliveryData(saleId, opticalStoreId)
            ??
            throw new NotFoundException(ResourceMessagesException.SALE_NOT_FOUND);

        if (sale.Status is not Domain.Enums.SaleStatus.Confirmed and not Domain.Enums.SaleStatus.Ready)
        {
            throw new ConflictException(ResourceMessagesException.SALE_CANNOT_BE_DELIVERED);
        }

        if (sale.ReceivedAmount < sale.TotalAmount)
            throw new ConflictException(ResourceMessagesException.SALE_HAS_OUTSTANDING_BALANCE);

        var saleUpdated = await _saleUpdateOnlyRepository.TryUpdateStatus(
            saleId,
            opticalStoreId,
            sale.Status,
            Domain.Enums.SaleStatus.Delivered
        );

        if (!saleUpdated)
            throw new ConflictException(ResourceMessagesException.SALE_CANNOT_BE_DELIVERED);
    }
}
