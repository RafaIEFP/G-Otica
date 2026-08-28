using GOtica.Communication.Response.Supplier;

namespace GOtica.Application.UseCases.Supplier.Get;

public interface IGetSupplierUseCase
{
    Task<ResponseSupplier> Execute(Guid opticalStoreId, Guid supplierId);
}
