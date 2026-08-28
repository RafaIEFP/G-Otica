using GOtica.Communication.Requests.Supplier;
using GOtica.Communication.Response.Supplier;

namespace GOtica.Application.UseCases.Supplier.Register;

public interface IRegisterSupplierUseCase
{
    Task<ResponseRegisterSupplier> Execute(Guid opticalStoreId, RequestRegisterSupplier request);
}
