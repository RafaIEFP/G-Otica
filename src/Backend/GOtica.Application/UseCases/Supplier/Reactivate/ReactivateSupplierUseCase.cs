using GOtica.Domain.Repositories.Supplier;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;

namespace GOtica.Application.UseCases.Supplier.Reactivate;

public class ReactivateSupplierUseCase : IReactivateSupplierUseCase
{
    private readonly ISupplierUpdateOnlyRepository _supplierUpdateOnlyRepository;
    public ReactivateSupplierUseCase(ISupplierUpdateOnlyRepository supplierUpdateOnlyRepository)
    {
        _supplierUpdateOnlyRepository = supplierUpdateOnlyRepository;
    }

    public async Task Execute(Guid opticalStoreId, Guid supplierId)
    {
        var reactivated = await _supplierUpdateOnlyRepository.Reactivate(supplierId, opticalStoreId);

        if (!reactivated)
            throw new NotFoundException(ResourceMessagesException.SUPPLIER_NOT_FOUND);
    }
}
