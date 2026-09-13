using GOtica.API.Attributes;
using GOtica.Application.UseCases.Treatment.Get;
using GOtica.Application.UseCases.Treatment.GetAll;
using GOtica.Application.UseCases.Treatment.Register;
using GOtica.Application.UseCases.Treatment.Update;
using GOtica.Communication.Requests.Treatment;
using GOtica.Communication.Response;
using GOtica.Communication.Response.Treatment;
using Microsoft.AspNetCore.Mvc;

namespace GOtica.API.Controllers;

[Route("api/optical-stores/{opticalStoreId:guid}/treatments")]
[ApiController]
[AuthenticatedUser]
[OpticalStoreMember]
public class TreatmentController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisterTreatment), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register(
        [FromRoute] Guid opticalStoreId,
        [FromBody] RequestTreatment request,
        [FromServices] IRegisterTreatmentUseCase useCase)
    {
        var response = await useCase.Execute(opticalStoreId, request);

        return Created(string.Empty, response);
    }

    [HttpGet("{treatmentId:guid}")]
    [ProducesResponseType(typeof(ResponseTreatment), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(
        [FromRoute] Guid opticalStoreId,
        [FromRoute] Guid treatmentId,
        [FromServices] IGetTreatmentUseCase useCase)
    {
        var response = await useCase.Execute(opticalStoreId, treatmentId);

        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponsePaged<ResponseTreatment>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll(
        [FromRoute] Guid opticalStoreId,
        [FromQuery] RequestGetAllTreatments request,
        [FromServices] IGetAllTreatmentsUseCase useCase)
    {
        var response = await useCase.Execute(opticalStoreId, request);

        return Ok(response);
    }

    [HttpPut("{treatmentId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid opticalStoreId,
        [FromRoute] Guid treatmentId,
        [FromBody] RequestTreatment request,
        [FromServices] IUpdateTreatmentUseCase useCase)
    {
        await useCase.Execute(opticalStoreId, treatmentId, request);

        return NoContent();
    }
}
