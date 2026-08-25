using GOtica.API.Attributes;
using GOtica.Application.UseCases.Client.Register;
using GOtica.Communication.Requests.Client;
using GOtica.Communication.Response;
using GOtica.Communication.Response.Client;
using Microsoft.AspNetCore.Mvc;

namespace GOtica.API.Controllers;

[Route("api/optical-stores/{opticalStoreId:guid}/clients")]
[ApiController]
[AuthenticatedUser]
public class ClientController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisterClient),StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseError),StatusCodes.Status400BadRequest)]
    [OpticalStoreMember]
    public async Task<IActionResult> Register(
        [FromRoute] Guid opticalStoreId,
        [FromBody] RequestRegisterClient request,
        [FromServices] IRegisterClientUseCase useCase)
    {
        var response = await useCase.Execute(opticalStoreId, request);

        return Created(string.Empty, response);
    }
}
