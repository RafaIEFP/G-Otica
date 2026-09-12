using GOtica.Domain.Enums;
using GOtica.Domain.Repositories;
using GOtica.Domain.Repositories.Payment;
using GOtica.Domain.Repositories.Product;
using GOtica.Domain.Repositories.Sale;
using GOtica.Domain.Repositories.StockMovement;
using GOtica.Domain.Services;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;

namespace GOtica.Application.UseCases.Sale.Cancel;

public class CancelSaleUseCase : ICancelSaleUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISaleReadOnlyRepository _saleReadOnlyRepository;
    private readonly ISaleUpdateOnlyRepository _saleUpdateOnlyRepository;
    private readonly IPaymentUpdateOnlyRepository _paymentUpdateOnlyRepository;
    private readonly IProductUpdateOnlyRepository _productUpdateOnlyRepository;
    private readonly IStockMovementWriteOnlyRepository _stockMovementWriteOnlyRepository;

    public CancelSaleUseCase(
        ILoggedUser loggedUser,
        IUnitOfWork unitOfWork,
        ISaleReadOnlyRepository saleReadOnlyRepository,
        ISaleUpdateOnlyRepository saleUpdateOnlyRepository,
        IPaymentUpdateOnlyRepository paymentUpdateOnlyRepository,
        IProductUpdateOnlyRepository productUpdateOnlyRepository,
        IStockMovementWriteOnlyRepository stockMovementWriteOnlyRepository)
    {
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
        _saleReadOnlyRepository = saleReadOnlyRepository;
        _saleUpdateOnlyRepository = saleUpdateOnlyRepository;
        _paymentUpdateOnlyRepository = paymentUpdateOnlyRepository;
        _productUpdateOnlyRepository = productUpdateOnlyRepository;
        _stockMovementWriteOnlyRepository = stockMovementWriteOnlyRepository;
    }

    public async Task Execute(Guid opticalStoreId, Guid saleId)
    {
        var loggedUser = await _loggedUser.Get();

        var sale = await _saleReadOnlyRepository.GetCancellationData(saleId, opticalStoreId)
            ??
            throw new NotFoundException(ResourceMessagesException.SALE_NOT_FOUND);

        // Cancellation status
        var canBeCancelled = sale.Status is
            SaleStatus.Confirmed or
            SaleStatus.InProduction or
            SaleStatus.Ready;

        if (!canBeCancelled)
            throw new ConflictException(ResourceMessagesException.SALE_CANNOT_BE_CANCELLED);

        // Stock impact grouped by product
        var quantitiesToRestore = sale.Items
            .GroupBy(item => item.ProductId)
            .ToDictionary(
                group => group.Key,
                group => group.Sum(item => item.Quantity));

        var now = DateTime.UtcNow;

        // Stock history
        var stockMovements = quantitiesToRestore
            .OrderBy(item => item.Key)
            .Select(item => new Domain.Entities.StockMovement
            {
                ProductId = item.Key,
                UserId = loggedUser.Id,
                QuantityChange = item.Value,
                Type = StockMovementType.SaleCancellation,
                Reason = null,
                CreatedAt = now
            })
            .ToList();


        await _unitOfWork.ExecuteInTransaction(async () =>
        {
            var saleCancelled =
                await _saleUpdateOnlyRepository.TryUpdateStatus(saleId, opticalStoreId, sale.Status, SaleStatus.Cancelled);

            if (!saleCancelled)
                throw new ConflictException(ResourceMessagesException.SALE_CANNOT_BE_CANCELLED);

            // Payments
            await _paymentUpdateOnlyRepository.CancelBySale(saleId, opticalStoreId);

            // Restore product stock
            foreach (var productQuantity in quantitiesToRestore.OrderBy(item => item.Key))
            {
                var stockRestored =
                    await _productUpdateOnlyRepository.RestoreStock(productQuantity.Key, opticalStoreId, productQuantity.Value);

                if (!stockRestored)
                    throw new ConflictException(ResourceMessagesException.SALE_CANNOT_BE_CANCELLED);
            }


            await _stockMovementWriteOnlyRepository.AddRange(stockMovements);
        });
    }
}
