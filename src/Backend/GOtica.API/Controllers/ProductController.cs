using GOtica.API.Attributes;
using GOtica.Application.UseCases.Product.Get;
using GOtica.Application.UseCases.Product.GetAll;
using GOtica.Application.UseCases.Product.Register;
using GOtica.Application.UseCases.Product.Update;
using GOtica.Communication.Requests.Product;
using GOtica.Communication.Response;
using GOtica.Communication.Response.Product;
using Microsoft.AspNetCore.Mvc;

namespace GOtica.API.Controllers;

[Route("api/optical-stores/{opticalStoreId:guid}/products")]
[ApiController]
[AuthenticatedUser]
public class ProductController : ControllerBase
{
    [HttpPost]
    [OpticalStoreMember]
    [ProducesResponseType(typeof(ResponseRegisterProduct), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register(
        [FromRoute] Guid opticalStoreId,
        [FromBody] RequestRegisterProduct request,
        [FromServices] IRegisterProductUseCase useCase)
    {
        var response = await useCase.Execute(opticalStoreId, request);

        return Created(string.Empty, response);
    }

    [HttpGet("{productId:guid}")]
    [OpticalStoreMember]
    [ProducesResponseType(typeof(ResponseGetProduct), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(
        [FromRoute] Guid opticalStoreId,
        [FromRoute] Guid productId,
        [FromServices] IGetProductUseCase useCase)
    {
        var response = await useCase.Execute(opticalStoreId, productId);

        return Ok(response);
    }

    [HttpGet]
    [OpticalStoreMember]
    [ProducesResponseType(typeof(ResponsePaged<ResponseGetAllProduct>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll(
        [FromRoute] Guid opticalStoreId,
        [FromQuery] RequestGetAllProducts request,
        [FromServices] IGetAllProductsUseCase useCase)
    {
        var response = await useCase.Execute(opticalStoreId,request);

        return Ok(response);
    }

    [HttpPut("{productId:guid}")]
    [OpticalStoreMember]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid opticalStoreId,
        [FromRoute] Guid productId,
        [FromBody] RequestUpdateProduct request,
        [FromServices] IUpdateProductUseCase useCase)
    {
        await useCase.Execute(
            opticalStoreId,
            productId,
            request);

        return NoContent();
    }
}
