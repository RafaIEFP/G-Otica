using GOtica.Application.UseCases.UserOpticalStore.Invite.Validade;
using GOtica.Communication.Response;
using GOtica.Communication.Response.UserOpticalStore;
using Microsoft.AspNetCore.Mvc;

namespace GOtica.API.Controllers;

[Route("api/invites")]
[ApiController]
public class InviteController : ControllerBase
{
    [HttpGet("validate", Name = "ValidateInvite")]
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
