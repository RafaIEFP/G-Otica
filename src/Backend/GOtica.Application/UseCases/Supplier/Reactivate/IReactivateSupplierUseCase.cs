namespace GOtica.Application.UseCases.Supplier.Reactivate;

public interface IReactivateSupplierUseCase
{
    Task Execute(Guid opticalStoreId, Guid supplierId);
}
