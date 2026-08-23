using GOtica.API.Attributes;
using GOtica.Application.UseCases.Invite.Accept;
using GOtica.Application.UseCases.UserOpticalStore.Invite.Validade;
using GOtica.Communication.Requests;
using GOtica.Communication.Response;
using GOtica.Communication.Response.Invite;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GOtica.API.Controllers;

[Route("api/invites")]
[ApiController]
[AuthenticatedUser]
public class InviteController : ControllerBase
{
    [HttpGet("validate", Name = "ValidateInvite")]
    [ProducesResponseType(typeof(ResponseValidateInvite), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status404NotFound)]
    [AllowAnonymous]
    public async Task<IActionResult> Validate(
        [FromQuery] string token,
        [FromServices] IValidateInviteUseCase useCase)
    {
        var response = await useCase.Execute(token);

        return Ok(response);
    }

    [HttpPost("accept")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Accept(
        [FromBody] RequestAcceptInvite request,
        [FromServices] IAcceptInviteUseCase useCase)
    {
        await useCase.Execute(request);

        return Ok();
    }
}
