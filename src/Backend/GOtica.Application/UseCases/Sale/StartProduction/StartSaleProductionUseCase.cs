using GOtica.Domain.Repositories.Sale;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;

namespace GOtica.Application.UseCases.Sale.StartProduction;

public class StartSaleProductionUseCase : IStartSaleProductionUseCase
{
    private readonly ISaleReadOnlyRepository _saleReadOnlyRepository;
    private readonly ISaleUpdateOnlyRepository _saleUpdateOnlyRepository;
    public StartSaleProductionUseCase(
        ISaleReadOnlyRepository saleReadOnlyRepository,
        ISaleUpdateOnlyRepository saleUpdateOnlyRepository)
    {
        _saleReadOnlyRepository = saleReadOnlyRepository;
        _saleUpdateOnlyRepository = saleUpdateOnlyRepository;
    }

    public async Task Execute(Guid opticalStoreId, Guid saleId)
    {
        var productionStarted = await _saleUpdateOnlyRepository.TryStartProduction(saleId, opticalStoreId);

        if (productionStarted)
            return;

        var saleExists = await _saleReadOnlyRepository.Exist(saleId, opticalStoreId);

        if (!saleExists)
            throw new NotFoundException(ResourceMessagesException.SALE_NOT_FOUND);

        throw new ConflictException(ResourceMessagesException.SALE_CANNOT_START_PRODUCTION);
    }
}
