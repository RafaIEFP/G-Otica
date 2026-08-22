using GOtica.API.Attributes;
using GOtica.Application.UseCases.UserOpticalStore.Invite.Create;
using GOtica.Application.UseCases.UserOpticalStore.Invite.Validade;
using GOtica.Communication.Requests;
using GOtica.Communication.Response;
using GOtica.Communication.Response.UserOpticalStore;
using Microsoft.AspNetCore.Mvc;

namespace GOtica.API.Controllers;

[Route("api/[controller]")]
[ApiController]

public class UserOpticalStoreController : ControllerBase
{
    [HttpPost]
    [Route("{opticalStoreId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    [AuthenticatedUser]
    [OwnerOnly]
    public async Task<IActionResult> Invite(
        [FromRoute] Guid opticalStoreId, 
        [FromBody] RequestInvite request, 
        [FromServices] ICreateInviteUseCase useCase)
    {
        await useCase.Execute(opticalStoreId, request);

        return Ok();
    }

    [HttpGet("validate")]
    [ProducesResponseType(typeof(ResponseValidateInvite), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Validate(
        [FromQuery] string token, 
        [FromServices] IValidateInviteUseCase useCase)
    {
        var response = await useCase.Execute(token);

        return Ok(response);
    }
}
