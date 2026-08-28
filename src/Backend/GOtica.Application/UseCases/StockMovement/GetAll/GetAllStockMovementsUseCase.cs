using GOtica.Communication.Requests.StockMovement;
using GOtica.Communication.Response;
using GOtica.Communication.Response.StockMovement;
using GOtica.Domain.Repositories.Product;
using GOtica.Domain.Repositories.StockMovement;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;
using Mapster;

namespace GOtica.Application.UseCases.StockMovement.GetAll;

public class GetAllStockMovementsUseCase : IGetAllStockMovementsUseCase
{
    private readonly IProductReadOnlyRepository _productReadOnlyRepository;
    private readonly IStockMovementReadOnlyRepository _stockMovementReadOnlyRepository;
    public GetAllStockMovementsUseCase(
        IProductReadOnlyRepository productReadOnlyRepository,
        IStockMovementReadOnlyRepository stockMovementReadOnlyRepository)
    {
        _productReadOnlyRepository = productReadOnlyRepository;
        _stockMovementReadOnlyRepository = stockMovementReadOnlyRepository;
    }

    public async Task<ResponsePaged<ResponseStockMovement>> Execute(Guid opticalStoreId, Guid productId, RequestGetStockMovements request)
    {
        Validate(request);

        var productExistsInOpticalStore = await _productReadOnlyRepository.Exists(productId, opticalStoreId);

        if (!productExistsInOpticalStore)
            throw new NotFoundException(ResourceMessagesException.PRODUCT_NOT_FOUND);

        var movements = await _stockMovementReadOnlyRepository.GetAll(productId, request.Page, request.PageSize);

        return new ResponsePaged<ResponseStockMovement>
        {
            Items = movements.Items
                .Adapt<IReadOnlyCollection<ResponseStockMovement>>(),

            Page = movements.Page,
            PageSize = movements.PageSize,
            TotalCount = movements.TotalCount
        };
    }

    private static void Validate(RequestGetStockMovements request)
    {
        var result = new GetStockMovementsValidator().Validate(request);

        if (!result.IsValid)
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
    }
}
