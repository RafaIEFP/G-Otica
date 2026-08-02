using System.Net;

namespace GOtica.Exceptions.ExceptionsBase;

public class ErrorOnValidationException(IList<string> listErrors) : GOticaException("")
{
    private readonly IList<string> _errorMenssages = listErrors;

    public override IList<string> GetErrorMessages() => _errorMenssages.Distinct().ToList();

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.BadRequest;
}