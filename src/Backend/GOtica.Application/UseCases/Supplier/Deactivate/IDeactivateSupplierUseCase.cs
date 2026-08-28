namespace GOtica.Application.UseCases.Supplier.Deactivate;

public interface IDeactivateSupplierUseCase
{
    Task Execute(Guid opticalStoreId, Guid supplierId);
}
