using GOtica.Domain.Repositories.Sale;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;

namespace GOtica.Application.UseCases.Sale.MarkAsReady;

public class MarkSaleAsReadyUseCase : IMarkSaleAsReadyUseCase
{
    private readonly ISaleReadOnlyRepository _saleReadOnlyRepository;
    private readonly ISaleUpdateOnlyRepository _saleUpdateOnlyRepository;
    public MarkSaleAsReadyUseCase(
        ISaleReadOnlyRepository saleReadOnlyRepository,
        ISaleUpdateOnlyRepository saleUpdateOnlyRepository)
    {
        _saleReadOnlyRepository = saleReadOnlyRepository;
        _saleUpdateOnlyRepository = saleUpdateOnlyRepository;
    }

    public async Task Execute(Guid opticalStoreId, Guid saleId)
    {
        var statusUpdated = await _saleUpdateOnlyRepository.TryUpdateStatus(
            saleId,
            opticalStoreId,
            Domain.Enums.SaleStatus.InProduction,
            Domain.Enums.SaleStatus.Ready);

        if (statusUpdated)
            return;

        var saleExists = await _saleReadOnlyRepository.Exist(saleId, opticalStoreId);

        if (!saleExists)
            throw new NotFoundException(ResourceMessagesException.SALE_NOT_FOUND);

        throw new ConflictException(ResourceMessagesException.SALE_CANNOT_BE_MARKED_AS_READY);
    }
}
