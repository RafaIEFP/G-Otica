using GOtica.API.Attributes;
using GOtica.Application.UseCases.Purchase.Register;
using GOtica.Communication.Requests.Purchase;
using GOtica.Communication.Response;
using GOtica.Communication.Response.Purchase;
using Microsoft.AspNetCore.Mvc;

namespace GOtica.API.Controllers;

[Route("api/optical-stores/{opticalStoreId:guid}/purchases")]
[ApiController]
[AuthenticatedUser]
public class PurchaseController : ControllerBase
{
    [HttpPost]
    [OpticalStoreMember]
    [ProducesResponseType(typeof(ResponseRegisterPurchase), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Register(
        [FromRoute] Guid opticalStoreId,
        [FromBody] RequestRegisterPurchase request,
        [FromServices] IRegisterPurchaseUseCase useCase)
    {
        var response = await useCase.Execute(opticalStoreId, request);

        return Created(string.Empty, response);
    }
}
