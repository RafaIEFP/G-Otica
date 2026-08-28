using GOtica.Communication.Requests.Supplier;

namespace GOtica.Application.UseCases.Supplier.Update;

public interface IUpdateSupplierUseCase
{
    Task Execute(Guid opticalStoreId, Guid supplierId, RequestUpdateSupplier request);
}
