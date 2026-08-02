using GOtica.Communication.Response;
using GOtica.Exceptions.ExceptionsBase;
using GOtica.Exceptions.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GOtica.API.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is GOticaException goticaException)
            HandleProjectException(goticaException, context);
        else
            ThrowUnknowError(context);
    }

    private static void HandleProjectException(GOticaException goticaException, ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)goticaException.GetStatusCode();
        context.Result = new ObjectResult(new ResponseError(goticaException.GetErrorMessages()));
    }

    private static void ThrowUnknowError(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(new ResponseError(ResourceMessagesException.UNKNOWN_ERROR));
    }
}
