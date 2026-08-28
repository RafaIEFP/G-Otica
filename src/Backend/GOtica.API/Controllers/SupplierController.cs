using GOtica.API.Attributes;
using GOtica.Application.UseCases.Supplier.Deactivate;
using GOtica.Application.UseCases.Supplier.Get;
using GOtica.Application.UseCases.Supplier.GetAll;
using GOtica.Application.UseCases.Supplier.Register;
using GOtica.Application.UseCases.Supplier.Update;
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

    [HttpGet("{supplierId:guid}")]
    [OpticalStoreMember]
    [ProducesResponseType(typeof(ResponseSupplier), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(
        [FromRoute] Guid opticalStoreId,
        [FromRoute] Guid supplierId,
        [FromServices] IGetSupplierUseCase useCase)
    {
        var response = await useCase.Execute(opticalStoreId, supplierId);

        return Ok(response);
    }

    [HttpGet]
    [OpticalStoreMember]
    [ProducesResponseType(typeof(ResponsePaged<ResponseSupplier>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromRoute] Guid opticalStoreId,
        [FromQuery] RequestGetAllSuppliers request,
        [FromServices] IGetAllSuppliersUseCase useCase)
    {
        var response = await useCase.Execute(opticalStoreId, request);

        return Ok(response);
    }

    [HttpPut("{supplierId:guid}")]
    [OpticalStoreMember]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid opticalStoreId,
        [FromRoute] Guid supplierId,
        [FromBody] RequestUpdateSupplier request,
        [FromServices] IUpdateSupplierUseCase useCase)
    {
        await useCase.Execute(opticalStoreId, supplierId, request);

        return NoContent();
    }

    [HttpDelete("{supplierId:guid}")]
    [OpticalStoreMember]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(
        [FromRoute] Guid opticalStoreId,
        [FromRoute] Guid supplierId,
        [FromServices] IDeactivateSupplierUseCase useCase)
    {
        await useCase.Execute(opticalStoreId, supplierId);

        return NoContent();
    }
}
