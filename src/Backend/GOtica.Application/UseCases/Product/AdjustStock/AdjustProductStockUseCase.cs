using GOtica.Communication.Enums;
using GOtica.Communication.Requests;
using GOtica.Communication.Requests.Product;
using GOtica.Domain.Repositories;
using GOtica.Domain.Repositories.Product;
using GOtica.Domain.Repositories.StockMovement;
using GOtica.Domain.Services;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;

namespace GOtica.Application.UseCases.Product.AdjustStock;

public class AdjustProductStockUseCase : IAdjustProductStockUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductReadOnlyRepository _productReadOnlyRepository;
    private readonly IProductUpdateOnlyRepository _productUpdateOnlyRepository;
    private readonly IStockMovementWriteOnlyRepository _stockMovementWriteOnlyRepository;
    public AdjustProductStockUseCase(
        ILoggedUser loggedUser,
        IUnitOfWork unitOfWork,
        IProductReadOnlyRepository productReadOnlyRepository,
        IProductUpdateOnlyRepository productUpdateOnlyRepository,
        IStockMovementWriteOnlyRepository stockMovementWriteOnlyRepository)
    {
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
        _productReadOnlyRepository = productReadOnlyRepository;
        _productUpdateOnlyRepository = productUpdateOnlyRepository;
        _stockMovementWriteOnlyRepository = stockMovementWriteOnlyRepository;
    }

    public async Task Execute(Guid opticalStoreId, Guid productId, RequestAdjustProductStock request)
    {
        request = request.Normalize();

        var loggedUser = await _loggedUser.Get();

        Validate(request);

        var product = await _productReadOnlyRepository.GetById(productId, opticalStoreId)
            ?? 
            throw new NotFoundException(ResourceMessagesException.PRODUCT_NOT_FOUND);

        var quantityChange = request.Type switch
        {
            StockAdjustmentType.Increase => request.Quantity,
            StockAdjustmentType.Decrease => -request.Quantity,
            _ => default!
        };

        if (quantityChange < 0 && 
            product.StockQuantity < request.Quantity)
                throw new ConflictException(ResourceMessagesException.INSUFFICIENT_STOCK);

        await _unitOfWork.ExecuteInTransaction(async () =>
        {
            var adjusted = await _productUpdateOnlyRepository
                .AdjustStock(
                    productId,
                    opticalStoreId,
                    quantityChange);

            if (!adjusted)
                throw new ConflictException(ResourceMessagesException.INSUFFICIENT_STOCK);

            var stockMovement = new Domain.Entities.StockMovement
            {
                QuantityChange = quantityChange,
                Type = Domain.Enums.StockMovementType.ManualAdjustment,
                Reason = request.Reason,
                ProductId = productId,
                UserId = loggedUser.Id
            };

            await _stockMovementWriteOnlyRepository.Add(stockMovement);
        });
    }

    private static void Validate(RequestAdjustProductStock request)
    {
        var result = new AdjustProductStockValidator().Validate(request);

        if (!result.IsValid)
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
    }
}
