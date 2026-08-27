using GOtica.Application.UseCases.Client.GetAll;
using GOtica.Communication.Requests.Client;
using GOtica.Communication.Requests.Product;
using GOtica.Communication.Response;
using GOtica.Communication.Response.Product;
using GOtica.Domain.Repositories.Product;
using GOtica.Exceptions.ExceptionsBase;
using Mapster;

namespace GOtica.Application.UseCases.Product.GetAll;

public class GetAllProductsUseCase : IGetAllProductsUseCase
{
    private readonly IProductReadOnlyRepository _productReadOnlyRepository;
    public GetAllProductsUseCase(IProductReadOnlyRepository productReadOnlyRepository)
    {
        _productReadOnlyRepository = productReadOnlyRepository;
    }

    public async Task<ResponsePaged<ResponseGetAllProduct>> Execute(Guid opticalStoreId, RequestGetAllProducts request)
    {
        Validate(request);

        var result = await _productReadOnlyRepository.GetAll(
            opticalStoreId,
            request.Page,
            request.PageSize,
            request.IsActive);

        return new ResponsePaged<ResponseGetAllProduct>
        {
            Items = result.Items.Adapt<IReadOnlyCollection<ResponseGetAllProduct>>(),

            Page = result.Page,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount,
            TotalPages = result.TotalPages
        };
    }

    private static void Validate(RequestGetAllProducts request)
    {
        var result = new GetAllProductsValidator().Validate(request);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
