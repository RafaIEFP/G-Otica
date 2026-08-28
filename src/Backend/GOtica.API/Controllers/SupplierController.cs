using GOtica.API.Attributes;
using GOtica.Application.UseCases.Supplier.Register;
using GOtica.Communication.Requests.Supplier;
using GOtica.Communication.Response;
using GOtica.Communication.Response.Supplier;
using Microsoft.AspNetCore.Mvc;

namespace GOtica.API.Controllers;

[Route("api/optical-stores/{opticalStoreId:guid}/suppliers")]
[ApiController]
[AuthenticatedUser]
public class SupplierController : ControllerBase
{
    [HttpPost]
    [OpticalStoreMember]
    [ProducesResponseType(typeof(ResponseRegisterSupplier), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromRoute] Guid opticalStoreId,
        [FromBody] RequestRegisterSupplier request,
        [FromServices] IRegisterSupplierUseCase useCase)
    {
        var response = await useCase.Execute(opticalStoreId, request);

        return Created(string.Empty, response);
    }
}
