using GOtica.Domain.Repositories.Supplier;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;

namespace GOtica.Application.UseCases.Supplier.Deactivate;

public class DeactivateSupplierUseCase : IDeactivateSupplierUseCase
{
    private readonly ISupplierUpdateOnlyRepository _supplierUpdateOnlyRepository;
    public DeactivateSupplierUseCase(ISupplierUpdateOnlyRepository supplierUpdateOnlyRepository)
    {
        _supplierUpdateOnlyRepository = supplierUpdateOnlyRepository;
    }

    public async Task Execute(Guid opticalStoreId, Guid supplierId)
    {
        var deactivated = await _supplierUpdateOnlyRepository.Deactivate(supplierId, opticalStoreId);

        if (!deactivated)
            throw new NotFoundException( ResourceMessagesException.SUPPLIER_NOT_FOUND);
    }
}
