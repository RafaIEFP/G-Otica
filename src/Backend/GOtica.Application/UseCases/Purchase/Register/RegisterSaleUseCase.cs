using GOtica.Application.UseCases.Sale.Register;
using GOtica.Communication.Requests.Sale;
using GOtica.Communication.Response.Sale;
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

namespace GOtica.Application.UseCases.Purchase.Register;

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

        return new ResponseRegisterSale
        {
            
        };
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
}
