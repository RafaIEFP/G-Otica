using GOtica.Communication.Response;
using GOtica.Communication.Response.Sale;
using GOtica.Domain.Repositories.Sale;
using GOtica.Exceptions.ExceptionsBase;
using Mapster;

namespace GOtica.Application.UseCases.Sale.GetAll;

public class GetAllSalesUseCase : IGetAllSalesUseCase
{
    private readonly ISaleReadOnlyRepository _saleReadOnlyRepository;
    public GetAllSalesUseCase(ISaleReadOnlyRepository saleReadOnlyRepository)
    {
        _saleReadOnlyRepository = saleReadOnlyRepository;
    }

    public async Task<ResponsePaged<ResponseGetAllSales>> Execute(Guid opticalStoreId, RequestGetAllSales request)
    {
        Validate(request);

        Domain.Enums.SaleStatus? status = request.Status.HasValue
            ? (Domain.Enums.SaleStatus)(int)request.Status.Value
            : null;

        var sales = await _saleReadOnlyRepository.GetAll(
            opticalStoreId,
            request.Page,
            request.PageSize,
            status);

        var items = sales.Items
            .Select(sale => sale.Adapt<ResponseGetAllSales>() with
            {
                RemainingAmount =
                    sale.Status == Domain.Enums.SaleStatus.Cancelled
                        ? 0
                        : sale.TotalAmount - sale.ReceivedAmount
            })
            .ToList();

        return new ResponsePaged<ResponseGetAllSales>
        {
            Items = items,

            Page = sales.Page,
            PageSize = sales.PageSize,
            TotalCount = sales.TotalCount,
            TotalPages = sales.TotalPages
        };
    }

    private static void Validate(RequestGetAllSales request)
    {
        var result = new GetAllSalesValidator().Validate(request);

        if (!result.IsValid)
            throw new ErrorOnValidationException(result.Errors.Select(error => error.ErrorMessage).ToList());
    }
}
