using GOtica.Communication.Requests;
using GOtica.Communication.Requests.Supplier;
using GOtica.Domain.Repositories;
using GOtica.Domain.Repositories.Supplier;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;

namespace GOtica.Application.UseCases.Supplier.Update;

public class UpdateSupplierUseCase : IUpdateSupplierUseCase
{
    private readonly ISupplierUpdateOnlyRepository _supplierUpdateOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    public UpdateSupplierUseCase(
        ISupplierUpdateOnlyRepository supplierUpdateOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _supplierUpdateOnlyRepository = supplierUpdateOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid opticalStoreId, Guid supplierId, RequestUpdateSupplier request)
    {
        request = request.Normalize();

        Validate(request);

        var supplier = await _supplierUpdateOnlyRepository.GetById(supplierId, opticalStoreId)
            ??
            throw new NotFoundException(ResourceMessagesException.SUPPLIER_NOT_FOUND);

        supplier.Name = request.Name;
        supplier.PhoneNumber = request.PhoneNumber;
        supplier.Email = request.Email;

        await _unitOfWork.Commit();
    }

    private static void Validate(RequestUpdateSupplier request)
    {
        var result = new UpdateSupplierValidator().Validate(request);

        if (!result.IsValid)
        {
            throw new ErrorOnValidationException(
                result.Errors
                    .Select(error => error.ErrorMessage)
                    .ToList());
        }
    }
}
