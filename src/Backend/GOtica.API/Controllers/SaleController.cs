using GOtica.API.Attributes;
using GOtica.Application.UseCases.Sale.Register;
using GOtica.Communication.Requests.Sale;
using GOtica.Communication.Response;
using GOtica.Communication.Response.Sale;
using Microsoft.AspNetCore.Mvc;

namespace GOtica.API.Controllers;

[Route("api/optical-stores/{opticalStoreId:guid}/sales")]
[ApiController]
[AuthenticatedUser]
[OpticalStoreMember]
public class SaleController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisterSale), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register(
        [FromRoute] Guid opticalStoreId,
        [FromBody] RequestRegisterSale request,
        [FromServices] IRegisterSaleUseCase useCase)
    {
        var response = await useCase.Execute(opticalStoreId, request);

        return Created(string.Empty, response);
    }
}
