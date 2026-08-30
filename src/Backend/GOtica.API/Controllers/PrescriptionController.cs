using GOtica.API.Attributes;
using GOtica.Application.UseCases.Prescription.Get;
using GOtica.Application.UseCases.Prescription.Register;
using GOtica.Communication.Requests.Prescription;
using GOtica.Communication.Response;
using GOtica.Communication.Response.Prescription;
using Microsoft.AspNetCore.Mvc;

namespace GOtica.API.Controllers;

[Route("api/optical-stores/{opticalStoreId:guid}/clients/{clientId:guid}/prescriptions")]
[ApiController]
[AuthenticatedUser]
[OpticalStoreMember]
public class PrescriptionController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisterPrescription), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Register(
        [FromRoute] Guid opticalStoreId,
        [FromRoute] Guid clientId,
        [FromBody] RequestRegisterPrescription request,
        [FromServices] IRegisterPrescriptionUseCase useCase)
    {
        var response = await useCase.Execute(opticalStoreId, clientId, request);

        return Created(string.Empty, response);
    }

    [HttpGet("{prescriptionId:guid}")]
    [ProducesResponseType(typeof(ResponseGetPrescription), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(
        [FromRoute] Guid opticalStoreId,
        [FromRoute] Guid clientId,
        [FromRoute] Guid prescriptionId,
        [FromServices] IGetPrescriptionUseCase useCase)
    {
        var response = await useCase.Execute(opticalStoreId, clientId, prescriptionId);

        return Ok(response);
    }
}
