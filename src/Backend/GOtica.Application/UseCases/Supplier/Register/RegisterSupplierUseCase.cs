using GOtica.Communication.Requests;
using GOtica.Communication.Requests.Supplier;
using GOtica.Communication.Response.Supplier;
using GOtica.Domain.Repositories;
using GOtica.Domain.Repositories.Supplier;
using GOtica.Exceptions.ExceptionsBase;
using Mapster;

namespace GOtica.Application.UseCases.Supplier.Register;

public class RegisterSupplierUseCase : IRegisterSupplierUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISupplierWriteOnlyRepository _supplierWriteOnlyRepository;
    public RegisterSupplierUseCase(
        IUnitOfWork unitOfWork,
        ISupplierWriteOnlyRepository supplierWriteOnlyRepository)
    {
        _unitOfWork = unitOfWork;
        _supplierWriteOnlyRepository = supplierWriteOnlyRepository;
    }

    public async Task<ResponseRegisterSupplier> Execute(Guid opticalStoreId, RequestRegisterSupplier request)
    {
        request = request.Normalize();

        Validate(request);

        var supplier = request.Adapt<Domain.Entities.Supplier>();

        supplier.OpticalStoreId = opticalStoreId;

        await _supplierWriteOnlyRepository.Add(supplier);

        await _unitOfWork.Commit();

        return supplier.Adapt<ResponseRegisterSupplier>();
    }

    private static void Validate(RequestRegisterSupplier request)
    {
        var result = new RegisterSupplierValidator().Validate(request);

        if (!result.IsValid)
        {
            throw new ErrorOnValidationException(
                result.Errors
                    .Select(error => error.ErrorMessage)
                    .ToList());
        }
    }
}
