using GOtica.Application.UseCases.User.ChangePassword;
using GOtica.Communication.Requests;
using GOtica.Communication.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GOtica.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "AuthenticatedUser")]
public class UserController : ControllerBase
{
    [HttpPut("change-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangePasswod([FromServices] IChangePasswordUseCase useCase, [FromBody] RequestChangePassword request)
    {
        await useCase.Execute(request);

        return NoContent();
    }
}
