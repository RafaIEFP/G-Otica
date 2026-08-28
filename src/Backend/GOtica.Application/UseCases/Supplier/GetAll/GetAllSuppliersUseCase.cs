using GOtica.Communication.Requests.Supplier;
using GOtica.Communication.Response;
using GOtica.Communication.Response.Supplier;
using GOtica.Domain.Repositories.Supplier;
using GOtica.Exceptions.ExceptionsBase;
using Mapster;

namespace GOtica.Application.UseCases.Supplier.GetAll;

public class GetAllSuppliersUseCase : IGetAllSuppliersUseCase
{
    private readonly ISupplierReadOnlyRepository _supplierReadOnlyRepository;
    public GetAllSuppliersUseCase(ISupplierReadOnlyRepository supplierReadOnlyRepository)
    {
        _supplierReadOnlyRepository = supplierReadOnlyRepository;
    }

    public async Task<ResponsePaged<ResponseSupplier>> Execute(Guid opticalStoreId, RequestGetAllSuppliers request)
    {
        Validate(request);

        var result = await _supplierReadOnlyRepository.GetAll(
            opticalStoreId,
            request.Page,
            request.PageSize,
            request.IsActive);

        return new ResponsePaged<ResponseSupplier>
        {
            Items = result.Items.Adapt<IReadOnlyCollection<ResponseSupplier>>(),

            Page = result.Page,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount,
            TotalPages = result.TotalPages
        };
    }

    private static void Validate(RequestGetAllSuppliers request)
    {
        var result = new GetAllSuppliersValidator().Validate(request);

        if (!result.IsValid)
        {
            throw new ErrorOnValidationException(
                result.Errors
                    .Select(error => error.ErrorMessage)
                    .ToList());
        }
    }
}
