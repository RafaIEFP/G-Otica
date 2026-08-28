using GOtica.Communication.Requests.Supplier;
using GOtica.Communication.Response;
using GOtica.Communication.Response.Supplier;

namespace GOtica.Application.UseCases.Supplier.GetAll;

public interface IGetAllSuppliersUseCase
{
    Task<ResponsePaged<ResponseSupplier>> Execute(Guid opticalStoreId, RequestGetAllSuppliers request);
}
