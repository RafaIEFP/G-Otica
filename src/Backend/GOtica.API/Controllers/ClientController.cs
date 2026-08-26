using GOtica.API.Attributes;
using GOtica.Application.UseCases.Client.Deactivate;
using GOtica.Application.UseCases.Client.Get;
using GOtica.Application.UseCases.Client.GetAll;
using GOtica.Application.UseCases.Client.Register;
using GOtica.Application.UseCases.Client.Update;
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

    [HttpGet("{clientId:guid}")]
    [ProducesResponseType(typeof(ResponseGetClient), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status404NotFound)]
    [OpticalStoreMember]
    public async Task<IActionResult> Get(
        [FromRoute] Guid opticalStoreId,
        [FromRoute] Guid clientId,
        [FromServices] IGetClientUseCase useCase)
    {
        var response = await useCase.Execute(opticalStoreId, clientId);

        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseGetAllClients), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    [OpticalStoreMember]
    public async Task<IActionResult> GetAll(
        [FromRoute] Guid opticalStoreId,
        [FromQuery] RequestGetAllClients request,
        [FromServices] IGetAllClientsUseCase useCase)
    {
        var response = await useCase.Execute(opticalStoreId, request);

        return Ok(response);
    }

    [HttpPut("{clientId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status404NotFound)]
    [OpticalStoreMember]
    public async Task<IActionResult> Update(
        [FromRoute] Guid opticalStoreId,
        [FromRoute] Guid clientId,
        [FromBody] RequestUpdateClient request,
        [FromServices] IUpdateClientUseCase useCase)
    {
        await useCase.Execute(opticalStoreId, clientId, request);

        return NoContent();
    }

    [HttpDelete("{clientId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status404NotFound)]
    [OpticalStoreMember]
    public async Task<IActionResult> Deactivate(
        [FromRoute] Guid opticalStoreId,
        [FromRoute] Guid clientId,
        [FromServices] IDeactivateClientUseCase useCase)
    {
        await useCase.Execute(opticalStoreId, clientId);

        return NoContent();
    }
}
