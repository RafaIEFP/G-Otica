using GOtica.API.Attributes;
using GOtica.Application.UseCases.OpticalStores.Deactivate;
using GOtica.Application.UseCases.OpticalStores.Register;
using GOtica.Application.UseCases.OpticalStores.TransferOwnership;
using GOtica.Application.UseCases.OpticalStores.Update;
using GOtica.Communication.Requests;
using GOtica.Communication.Response;
using Microsoft.AspNetCore.Mvc;

namespace GOtica.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[AuthenticatedUser]
public class OpticalStoreController : ControllerBase
{
    [HttpPut]
    [Route("{opticalStoreId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    [OwnerOnly]
    public async Task<IActionResult> TransferOwnership(
        [FromServices] ITransferOpticalStoreOwnershipUseCase useCase,
        [FromRoute] Guid opticalStoreId,
        [FromBody] Guid newOwnerId)
    {
        await useCase.Execute(newOwnerId, opticalStoreId);

        return NoContent();
    }

    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisterOpticalStore), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register(
        [FromServices] IRegisterOpticalStoreUseCase useCase, 
        [FromBody] RequestOpticalStore request)
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }

    [HttpDelete]
    [Route("{opticalStoreId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [OwnerOnly]
    public async Task<IActionResult> Deactivate(
        [FromServices] IDeactivateOpticalStoreUseCase useCase,
        [FromRoute] Guid opticalStoreId)
    {
        await useCase.Execute(opticalStoreId);

        return NoContent();
    }

    [HttpPut]
    [Route("Update/{opticalStoreId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status409Conflict)]
    [OwnerOnly]
    public async Task<IActionResult> Update(
        [FromServices] IUpdateOpticalStoreUseCase useCase,
        [FromRoute] Guid opticalStoreId,
        [FromBody] RequestOpticalStore request)
    {
        await useCase.Execute(opticalStoreId, request);

        return NoContent();
    }
}
