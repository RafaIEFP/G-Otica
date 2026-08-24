using GOtica.API.Attributes;
using GOtica.Application.UseCases.OpticalStores.Deactivate;
using GOtica.Application.UseCases.OpticalStores.Get;
using GOtica.Application.UseCases.OpticalStores.GetAll;
using GOtica.Application.UseCases.OpticalStores.Register;
using GOtica.Application.UseCases.OpticalStores.TransferOwnership;
using GOtica.Application.UseCases.OpticalStores.Update;
using GOtica.Application.UseCases.UserOpticalStore.Invite.Create;
using GOtica.Communication.Requests.Invite;
using GOtica.Communication.Requests.OpticalStore;
using GOtica.Communication.Response;
using GOtica.Communication.Response.OpticalStore;
using Microsoft.AspNetCore.Mvc;

namespace GOtica.API.Controllers;

[Route("api/optical-stores")]
[ApiController]
[AuthenticatedUser]
public class OpticalStoreController : ControllerBase
{
    [HttpPut("{opticalStoreId:guid}/ownership")]
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

    [HttpDelete("{opticalStoreId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [OwnerOnly]
    public async Task<IActionResult> Deactivate(
        [FromServices] IDeactivateOpticalStoreUseCase useCase,
        [FromRoute] Guid opticalStoreId)
    {
        await useCase.Execute(opticalStoreId);

        return NoContent();
    }

    [HttpPut("{opticalStoreId:guid}")]
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

    [HttpGet("{opticalStoreId:guid}")]
    [ProducesResponseType(typeof(ResponseGetOpticalStore), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(
        [FromServices] IGetOpticalStoreUseCase useCase,
        [FromRoute] Guid opticalStoreId)
    {
        var response = await useCase.Execute(opticalStoreId);

        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<ResponseGetAllOpticalStores>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAll(
        [FromServices] IGetAllOpticalStoresUseCase useCase)
    {
        var response = await useCase.Execute();

        if (response.Count != 0)
            return Ok(response);

        return NoContent();
    }

    [HttpPost("{opticalStoreId:guid}/invites")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    [OwnerOnly]
    public async Task<IActionResult> Invite(
        [FromRoute] Guid opticalStoreId,
        [FromBody] RequestInvite request,
        [FromServices] ICreateInviteUseCase useCase)
    {
        await useCase.Execute(opticalStoreId, request);

        return Ok();
    }
}
