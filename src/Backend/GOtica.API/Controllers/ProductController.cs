using GOtica.API.Attributes;
using GOtica.Application.UseCases.Product.Register;
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
}
