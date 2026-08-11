using GOtica.Application.UseCases.OpticalStores.TransferOwnership;
using GOtica.Communication.Response;
using Microsoft.AspNetCore.Mvc;

namespace GOtica.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OpticalStoreController : ControllerBase
{
    [HttpPut]
    [Route("{opticalId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> TransferOwnership(
        [FromServices] ITransferOpticalStoreOwnershipUseCase useCase,
        [FromRoute] long opticalId,
        [FromBody] Guid newOwnerId)
    {
        await useCase.Execute(newOwnerId, opticalId);

        return NoContent();
    }
}
