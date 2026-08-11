using System.Net;

namespace GOtica.Exceptions.ExceptionsBase;

public class UnauthorizedException(string mensagem) : GOticaException(mensagem)
{
    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}
