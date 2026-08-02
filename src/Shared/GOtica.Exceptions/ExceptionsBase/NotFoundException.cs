using System.Net;

namespace GOtica.Exceptions.ExceptionsBase;

public class NotFoundException(string mensagem) : GOticaException(mensagem)
{
    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.NotFound;
}
