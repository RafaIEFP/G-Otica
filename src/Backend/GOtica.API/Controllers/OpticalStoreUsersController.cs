using GOtica.API.Attributes;
using GOtica.Application.UseCases.UserOpticalStores.ChangeRole;
using GOtica.Application.UseCases.UserOpticalStores.GetAll;
using GOtica.Communication.Requests;
using GOtica.Communication.Response;
using GOtica.Communication.Response.UserOpticalStore;
using Microsoft.AspNetCore.Mvc;

namespace GOtica.API.Controllers;

[Route("api/optical-stores/{opticalStoreId:guid}/users")]
[ApiController]
[AuthenticatedUser]
public class OpticalStoreUsersController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<ResponseGetAllOpticalStoreUser>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [OpticalStoreMember]
    public async Task<IActionResult> GetAll(
        [FromRoute] Guid opticalStoreId,
        [FromServices] IGetAllOpticalStoreUsersUseCase useCase)
    {
        var response = await useCase.Execute(opticalStoreId);

        if (response.Count != 0)
            return Ok(response);

        return NoContent();
    }

    [HttpPut("{userId:guid}/role")]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [OwnerOnly]
    public async Task<IActionResult> ChangeRole(
        [FromRoute] Guid opticalStoreId,
        [FromRoute] Guid userId,
        [FromBody] RequestChangeRole request,
        [FromServices] IChangeRoleUseCase useCase)
    {
        await useCase.Execute(opticalStoreId, userId, request);

        return NoContent();
    }
}
