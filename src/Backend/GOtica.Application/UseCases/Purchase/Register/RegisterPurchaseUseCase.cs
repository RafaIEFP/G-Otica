using GOtica.Communication.Requests.Purchase;
using GOtica.Communication.Response.Purchase;
using GOtica.Domain.Entities;
using GOtica.Domain.Enums;
using GOtica.Domain.Repositories;
using GOtica.Domain.Repositories.Product;
using GOtica.Domain.Repositories.Purchase;
using GOtica.Domain.Repositories.StockMovement;
using GOtica.Domain.Repositories.Supplier;
using GOtica.Domain.Services;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;
using Mapster;

namespace GOtica.Application.UseCases.Purchase.Register;

public class RegisterPurchaseUseCase : IRegisterPurchaseUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISupplierReadOnlyRepository _supplierReadOnlyRepository;
    private readonly IProductReadOnlyRepository _productReadOnlyRepository;
    private readonly IProductUpdateOnlyRepository _productUpdateOnlyRepository;
    private readonly IPurchaseWriteOnlyRepository _purchaseWriteOnlyRepository;
    private readonly IStockMovementWriteOnlyRepository _stockMovementWriteOnlyRepository;
    public RegisterPurchaseUseCase(
        ILoggedUser loggedUser,
        IUnitOfWork unitOfWork,
        ISupplierReadOnlyRepository supplierReadOnlyRepository,
        IProductReadOnlyRepository productReadOnlyRepository,
        IProductUpdateOnlyRepository productUpdateOnlyRepository,
        IPurchaseWriteOnlyRepository purchaseWriteOnlyRepository,
        IStockMovementWriteOnlyRepository stockMovementWriteOnlyRepository)
    {
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
        _supplierReadOnlyRepository = supplierReadOnlyRepository;
        _productReadOnlyRepository = productReadOnlyRepository;
        _productUpdateOnlyRepository = productUpdateOnlyRepository;
        _purchaseWriteOnlyRepository = purchaseWriteOnlyRepository;
        _stockMovementWriteOnlyRepository = stockMovementWriteOnlyRepository;
    }

    public async Task<ResponseRegisterPurchase> Execute(Guid opticalStoreId, RequestRegisterPurchase request)
    {
        var loggedUser = await _loggedUser.Get();

        Validate(request);

        var supplierExists = await _supplierReadOnlyRepository.ExistsActiveSupplier(request.SupplierId, opticalStoreId);

        if (!supplierExists)
            throw new NotFoundException(ResourceMessagesException.SUPPLIER_NOT_FOUND);

        // Unique products
        var productIds = request.Items.Select(item => item.ProductId).Distinct().ToList();

        await ValidateProducts(productIds, opticalStoreId);

        var purchase = CreatePurchase(request, opticalStoreId, loggedUser.Id);

        // Stock impact grouped by product
        var stockChanges = request.Items
            .GroupBy(i => i.ProductId)
            .Select(group => new 
            {
                ProductId = group.Key,
                Quantity = group.Sum(item => item.Quantity)
            })
            .ToList();

        await _unitOfWork.ExecuteInTransaction(async () =>
        {
            await _purchaseWriteOnlyRepository.Add(purchase);

            foreach (var stockChange in stockChanges)
            {
                var adjusted = await _productUpdateOnlyRepository.AdjustStock(
                    stockChange.ProductId,
                    opticalStoreId,
                    stockChange.Quantity);

                if (!adjusted)
                    throw new NotFoundException(ResourceMessagesException.PRODUCT_NOT_FOUND);

                var stockMovement = new Domain.Entities.StockMovement
                {
                    ProductId = stockChange.ProductId,
                    UserId = loggedUser.Id,
                    QuantityChange = stockChange.Quantity,
                    Type = StockMovementType.Purchase
                };

                await _stockMovementWriteOnlyRepository.Add(stockMovement);
            }
        });

        return purchase.Adapt<ResponseRegisterPurchase>();
    }

    private static void Validate(RequestRegisterPurchase request)
    {
        var result = new RegisterPurchaseValidator().Validate(request);

        if (!result.IsValid)
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
    }

    private static Domain.Entities.Purchase CreatePurchase(RequestRegisterPurchase request, Guid opticalStoreId, Guid userId)
    {
        var purchase = new Domain.Entities.Purchase
        {
            SupplierId = request.SupplierId,
            OpticalStoreId = opticalStoreId,
            UserId = userId
        };

        foreach (var item in request.Items)
        {
            purchase.Items.Add(new PurchaseItem
            {
                PurchaseId = purchase.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                TotalAmount = item.Quantity * item.UnitPrice
            });
        }

        purchase.TotalAmount = purchase.Items.Sum(i => i.TotalAmount);

        return purchase;
    }

    private async Task ValidateProducts(IReadOnlyCollection<Guid> productIds, Guid opticalStoreId)
    {
        // Valid products from current store
        var validProductIds = await _productReadOnlyRepository.GetActiveProductIds(productIds, opticalStoreId);

        // Missing / inactive / another store
        var invalidProductIds = productIds.Except(validProductIds).ToList();

        if (invalidProductIds.Count != 0)
        {
            var productIdsText = string.Join(", ", invalidProductIds);

            throw new NotFoundException(
                string.Format(ResourceMessagesException.PRODUCTS_NOT_FOUND, productIdsText)
                );
        }
    }
}
