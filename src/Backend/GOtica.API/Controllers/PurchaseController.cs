using GOtica.API.Attributes;
using GOtica.Application.UseCases.Purchase.Get;
using GOtica.Application.UseCases.Purchase.GetAll;
using GOtica.Application.UseCases.Purchase.Register;
using GOtica.Communication.Requests.Purchase;
using GOtica.Communication.Response;
using GOtica.Communication.Response.Purchase;
using Microsoft.AspNetCore.Mvc;

namespace GOtica.API.Controllers;

[Route("api/optical-stores/{opticalStoreId:guid}/purchases")]
[ApiController]
[AuthenticatedUser]
[OpticalStoreMember]
public class PurchaseController : ControllerBase
{
    [HttpPost]
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

    [HttpGet("{purchaseId:guid}")]
    [ProducesResponseType(typeof(ResponseGetPurchase), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(
        [FromRoute] Guid opticalStoreId,
        [FromRoute] Guid purchaseId,
        [FromServices] IGetPurchaseUseCase useCase)
    {
        var response = await useCase.Execute(opticalStoreId, purchaseId);

        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponsePaged<ResponseGetAllPurchase>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll(
        [FromRoute] Guid opticalStoreId,
        [FromQuery] RequestGetAllPurchases request,
        [FromServices] IGetAllPurchasesUseCase useCase)
    {
        var response = await useCase.Execute(opticalStoreId, request);

        return Ok(response);
    }
}
