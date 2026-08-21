using GOtica.API.Attributes;
using GOtica.Application.UseCases.UserOpticalStore.Invite;
using GOtica.Communication.Requests;
using Microsoft.AspNetCore.Mvc;

namespace GOtica.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[AuthenticatedUser]
[OwnerOnly]
public class UserOpticalStoreController : ControllerBase
{
    [HttpPost]
    [Route("{opticalStoreId}")]
    public async Task<IActionResult> Invite(
        [FromRoute] Guid opticalStoreId, 
        [FromBody] RequestInvite request, 
        [FromServices] ICreateInviteUseCase useCase)
    {
        await useCase.Execute(opticalStoreId, request);

        return Ok();
    }
}
