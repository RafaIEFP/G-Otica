using GOtica.API.Attributes;
using GOtica.Application.UseCases.UserOpticalStores.GetAll;
using GOtica.Communication.Response.UserOpticalStore;
using Microsoft.AspNetCore.Mvc;

namespace GOtica.API.Controllers;

[Route("api/optical-stores/{opticalStoreId:guid}/users")]
[ApiController]
[AuthenticatedUser]
public class OpticalStoreUsersController : ControllerBase
{
    [HttpGet()]
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
}
