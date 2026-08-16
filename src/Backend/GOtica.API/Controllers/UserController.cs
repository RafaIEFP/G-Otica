using GOtica.API.Attributes;
using GOtica.Application.UseCases.User.ChangePassword;
using GOtica.Application.UseCases.User.DeleteAccount;
using GOtica.Application.UseCases.User.Profile;
using GOtica.Application.UseCases.User.Update;
using GOtica.Communication.Requests;
using GOtica.Communication.Response;
using GOtica.Communication.Response.User;
using Microsoft.AspNetCore.Mvc;

namespace GOtica.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[AuthenticatedUser]
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

    [HttpGet]
    [ProducesResponseType(typeof(ResponseUserProfile), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProfile([FromServices] IGetUserProfileUseCase useCase)
    {
        var response = await useCase.Execute();

        return Ok(response);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateProfile([FromServices] IUpdateUserUseCase useCase, [FromBody] RequestUpdateUser request)
    {
        await useCase.Execute(request);

        return NoContent();
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> DeleteAccount([FromServices] IDeleteAccountUseCase useCase)
    {
        await useCase.Execute();

        return NoContent();
    }
}
