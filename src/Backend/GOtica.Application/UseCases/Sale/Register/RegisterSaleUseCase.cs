using GOtica.Communication.Requests.Payment;
using GOtica.Communication.Requests.Sale;
using GOtica.Communication.Response.Sale;
using GOtica.Domain.Entities;
using GOtica.Domain.Enums;
using GOtica.Domain.Repositories;
using GOtica.Domain.Repositories.Client;
using GOtica.Domain.Repositories.Payment;
using GOtica.Domain.Repositories.Prescription;
using GOtica.Domain.Repositories.Product;
using GOtica.Domain.Repositories.Sale;
using GOtica.Domain.Repositories.StockMovement;
using GOtica.Domain.Services;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;
using Mapster;

namespace GOtica.Application.UseCases.Sale.Register;

public class RegisterSaleUseCase : IRegisterSaleUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClientReadOnlyRepository _clientReadOnlyRepository;
    private readonly IPrescriptionReadOnlyRepository _prescriptionReadOnlyRepository;
    private readonly IProductReadOnlyRepository _productReadOnlyRepository;
    private readonly IProductUpdateOnlyRepository _productUpdateOnlyRepository;
    private readonly ISaleWriteOnlyRepository _saleWriteOnlyRepository;
    private readonly IPaymentWriteOnlyRepository _paymentWriteOnlyRepository;
    private readonly IStockMovementWriteOnlyRepository _stockMovementWriteOnlyRepository;

    public RegisterSaleUseCase(
        ILoggedUser loggedUser,
        IUnitOfWork unitOfWork,
        IClientReadOnlyRepository clientReadOnlyRepository,
        IPrescriptionReadOnlyRepository prescriptionReadOnlyRepository,
        IProductReadOnlyRepository productReadOnlyRepository,
        IProductUpdateOnlyRepository productUpdateOnlyRepository,
        ISaleWriteOnlyRepository saleWriteOnlyRepository,
        IPaymentWriteOnlyRepository paymentWriteOnlyRepository,
        IStockMovementWriteOnlyRepository stockMovementWriteOnlyRepository)
    {
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
        _clientReadOnlyRepository = clientReadOnlyRepository;
        _prescriptionReadOnlyRepository = prescriptionReadOnlyRepository;
        _productReadOnlyRepository = productReadOnlyRepository;
        _productUpdateOnlyRepository = productUpdateOnlyRepository;
        _saleWriteOnlyRepository = saleWriteOnlyRepository;
        _paymentWriteOnlyRepository = paymentWriteOnlyRepository;
        _stockMovementWriteOnlyRepository = stockMovementWriteOnlyRepository;
    }

    public async Task<ResponseRegisterSale> Execute(Guid opticalStoreId, RequestRegisterSale request)
    {
        var loggedUser = await _loggedUser.Get();

        Validate(request);

        var clientExist = await _clientReadOnlyRepository.ExistActive(request.ClientId, opticalStoreId);

        if (!clientExist)
            throw new NotFoundException(ResourceMessagesException.CLIENT_NOT_FOUND);

        await ValidatePrescription(request.PrescriptionId, request.ClientId, opticalStoreId);

        var requestedQuantities = request.Items
            .GroupBy(r => r.ProductId)
            .ToDictionary(
                g => g.Key, 
                g => g.Sum(r => r.Quantity)
            );

        var productsIds = requestedQuantities.Keys.ToList();

        var products = await _productReadOnlyRepository.GetActivesByIds(productsIds, opticalStoreId);

        ValidateProducts(products, productsIds, requestedQuantities);

        var productsById = products.ToDictionary(p => p.Id);

        var now = DateTime.UtcNow;

        var sale = CreateSale(opticalStoreId, request, loggedUser.Id, productsById, now);

        ValidateInitialPayment(request.InitialPayment.Amount, sale.TotalAmount);

        var payments = CreatePayments(sale, request.InitialPayment, loggedUser.Id, now);

        var stockMovements = CreateStockMovements(requestedQuantities, loggedUser.Id, now);

        await _unitOfWork.ExecuteInTransaction(async () =>
        {
            await _saleWriteOnlyRepository.Add(sale);

            await _paymentWriteOnlyRepository.AddRange(payments);

            // Decrease product stock
            foreach (var productQuantity in requestedQuantities.OrderBy(item => item.Key))
            {
                var stockDecreased =
                    await _productUpdateOnlyRepository.TryDecreaseStock(productQuantity.Key, opticalStoreId, productQuantity.Value);

                if (!stockDecreased)
                    throw new ConflictException(ResourceMessagesException.INSUFFICIENT_PRODUCT_STOCK);
            }

            await _stockMovementWriteOnlyRepository.AddRange(stockMovements);
        });

        return sale.Adapt<ResponseRegisterSale>();
    }

    private static void Validate(RequestRegisterSale request)
    {
        var result = new RegisterSaleValidator().Validate(request);

        if (!result.IsValid)
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
    }

    private async Task ValidatePrescription(Guid? prescriptionId, Guid clientId, Guid opticalStoreId)
    {
        if (!prescriptionId.HasValue)
            return;

        var prescription = await _prescriptionReadOnlyRepository.GetById(prescriptionId.Value, clientId, opticalStoreId)
            ??
            throw new NotFoundException(ResourceMessagesException.PRESCRIPTION_NOT_FOUND);

        if (prescription.ExpirationDate < DateOnly.FromDateTime(DateTime.Now))
            throw new ConflictException(ResourceMessagesException.PRESCRIPTION_EXPIRED);
    }

    private static void ValidateProducts(
        IReadOnlyCollection<Domain.Entities.Product> products, 
        IReadOnlyCollection<Guid> productsIds, 
        IReadOnlyDictionary<Guid, int> requestedQuantities)
    {
        if (products.Count != productsIds.Count)
            throw new NotFoundException(ResourceMessagesException.PRODUCT_NOT_FOUND);

        foreach (var product in products)
        {
            var requestedQuantity = requestedQuantities[product.Id];

            if (product.StockQuantity < requestedQuantity)
                throw new ConflictException(ResourceMessagesException.INSUFFICIENT_PRODUCT_STOCK);
        }
    }

    private static Domain.Entities.Sale CreateSale(
        Guid opticalStoreId, 
        RequestRegisterSale request,
        Guid userId,
        IReadOnlyDictionary<Guid, Domain.Entities.Product> products,
        DateTime now)
    {
        var sale = new Domain.Entities.Sale
        {
            OpticalStoreId = opticalStoreId,
            ClientId = request.ClientId,
            UserId = userId,
            PrescriptionId = request.PrescriptionId,
            Status = SaleStatus.Confirmed,
            CreatedAt = now
        };

        foreach (var requestItem in request.Items)
        {
            var product = products[requestItem.ProductId];

            var grossAmount = product.BasePrice * requestItem.Quantity;

            ValidateDiscount(requestItem.DiscountAmount, grossAmount);

            var totalAmount = grossAmount - requestItem.DiscountAmount;

            sale.Items.Add(new SaleItem
            {
                Quantity = requestItem.Quantity,

                // Historical price
                UnitPrice = product.BasePrice,

                DiscountAmount = requestItem.DiscountAmount,
                TotalAmount = totalAmount,
                Notes = requestItem.Notes,
                ProductId = product.Id,
                SaleId = sale.Id,
                Sale = sale
            });

            sale.TotalAmount += totalAmount;
        }

        return sale;
    }

    private static void ValidateDiscount(decimal discountAmount, decimal grossAmount)
    {
        if (discountAmount >= grossAmount)
            throw new ErrorOnValidationException([ResourceMessagesException.SALE_ITEM_DISCOUNT_INVALID]);
    }

    private static void ValidateInitialPayment(decimal initialPaymentAmount, decimal saleTotalAmount)
    {
        if (initialPaymentAmount > saleTotalAmount)
            throw new ErrorOnValidationException([ResourceMessagesException.INITIAL_PAYMENT_GREATER_THAN_SALE_TOTAL]);
    }

    private static IReadOnlyCollection<Payment> CreatePayments(
        Domain.Entities.Sale sale,
        RequestRegisterSalePayment request,
        Guid userId,
        DateTime now)
    {
        var payments = new List<Payment>();

        // Initial payment - always received
        var initialPayment = new Payment
        {
            Amount = request.Amount,
            PaymentMethod = (PaymentMethod)request.PaymentMethod,
            Status = PaymentStatus.Received,
            ReceivedAt = now,
            SaleId = sale.Id,
            Sale = sale,
            ReceivedByUserId = userId
        };

        payments.Add(initialPayment);

        var remainingAmount = sale.TotalAmount - request.Amount;

        if (remainingAmount > 0)
        {
            // Remaining payment - expected
            var remainingPayment = new Payment
            {
                Amount = remainingAmount,
                PaymentMethod = null,
                Status = PaymentStatus.Pending,
                ReceivedAt = null,
                SaleId = sale.Id,
                Sale = sale,
                ReceivedByUserId = null
            };

            payments.Add(remainingPayment);
        }

        return payments;
    }

    private static IReadOnlyCollection<Domain.Entities.StockMovement> CreateStockMovements(
        IReadOnlyDictionary<Guid, int> requestedQuantities,
        Guid userId,
        DateTime now)
    {
        return requestedQuantities
            .OrderBy(item => item.Key)
            .Select(item => new Domain.Entities.StockMovement
            {
                ProductId = item.Key,
                UserId = userId,
                QuantityChange = -item.Value,
                Type = StockMovementType.Sale,
                Reason = null,
                CreatedAt = now
            })
            .ToList();
    }
}