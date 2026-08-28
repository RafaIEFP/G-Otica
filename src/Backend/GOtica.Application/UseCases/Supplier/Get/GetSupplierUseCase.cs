using GOtica.Communication.Response.Supplier;
using GOtica.Domain.Repositories.Supplier;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;
using Mapster;

namespace GOtica.Application.UseCases.Supplier.Get;

public class GetSupplierUseCase : IGetSupplierUseCase
{
    private readonly ISupplierReadOnlyRepository _supplierReadOnlyRepository;
    public GetSupplierUseCase(ISupplierReadOnlyRepository supplierReadOnlyRepository)
    {
        _supplierReadOnlyRepository = supplierReadOnlyRepository;
    }

    public async Task<ResponseSupplier> Execute(Guid opticalStoreId, Guid supplierId)
    {
        var supplier = await _supplierReadOnlyRepository.GetById(supplierId, opticalStoreId)
            ??
            throw new NotFoundException(ResourceMessagesException.SUPPLIER_NOT_FOUND);

        return supplier.Adapt<ResponseSupplier>();
    }
}
