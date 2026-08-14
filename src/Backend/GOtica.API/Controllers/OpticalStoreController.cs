using GOtica.API.Attributes;
using GOtica.Application.UseCases.OpticalStores.Register;
using GOtica.Application.UseCases.OpticalStores.TransferOwnership;
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
    [Route("{opticalId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    [OwnerOnly]
    public async Task<IActionResult> TransferOwnership(
        [FromServices] ITransferOpticalStoreOwnershipUseCase useCase,
        [FromRoute] Guid opticalId,
        [FromBody] Guid newOwnerId)
    {
        await useCase.Execute(newOwnerId, opticalId);

        return NoContent();
    }

    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisterOpticalStore), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> TransferOwnership(
        [FromServices] IRegisterOpticalStoreUseCase useCase, 
        [FromBody] RequestRegisterOpticalStore request)
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }
}
