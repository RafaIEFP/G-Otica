using GOtica.API.Attributes;
using GOtica.Application.UseCases.Login.DoLogin;
using GOtica.Application.UseCases.Login.DoLogout;
using GOtica.Application.UseCases.Token.RefreshToken;
using GOtica.Application.UseCases.User.Register;
using GOtica.Communication.Requests;
using GOtica.Communication.Response;
using GOtica.Communication.Response.User;
using Microsoft.AspNetCore.Mvc;

namespace GOtica.API.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    [HttpPost("register")]
    [ProducesResponseType(typeof(ResponseRegisteredUser), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromServices] IRegisterUserUseCase useCase, [FromBody] RequestRegisterUser request)
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(ResponseRegisteredUser), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromServices] IDoLoginUseCase useCase, [FromBody] RequestLogin request)
    {
        var response = await useCase.Execute(request);

        return Ok(response);
    }

    [HttpPost("refresh")]
    [ProducesResponseType(typeof(ResponseTokens), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh([FromServices] IRefreshTokenUseCase useCase, [FromBody] RequestNewToken request)
    {
        var response = await useCase.Execute(request);

        return Ok(response);
    }

    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [AuthenticatedUser]
    public async Task<IActionResult> Logout(
    [FromServices] IDoLogoutUseCase useCase)
    {
        await useCase.Execute();

        return NoContent();
    }
}
