using GOtica.Communication.Response.Sale;
using GOtica.Domain.Repositories.Sale;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;
using Mapster;

namespace GOtica.Application.UseCases.Sale.Get;

public class GetSaleUseCase : IGetSaleUseCase
{
    private readonly ISaleReadOnlyRepository _saleReadOnlyRepository;
    public GetSaleUseCase(ISaleReadOnlyRepository saleReadOnlyRepository)
        => _saleReadOnlyRepository = saleReadOnlyRepository;

    public async Task<ResponseGetSale> Execute(Guid saleId, Guid opticalStoreId)
    {
        var sale = await _saleReadOnlyRepository.GetById(saleId, opticalStoreId)
            ??
            throw new NotFoundException(ResourceMessagesException.SALE_NOT_FOUND);

        var receivedAmount = sale.Payments
            .Where(p => p.Status == Domain.Enums.PaymentStatus.Received)
            .Sum(p => p.Amount);

        var remainingAmount =
            sale.Status == Domain.Enums.SaleStatus.Cancelled
                ? 0 
                : sale.TotalAmount - receivedAmount;

        return sale.Adapt<ResponseGetSale>() with
        {
            ReceivedAmount = receivedAmount,
            RemainingAmount = remainingAmount
        };
    }
}
