using System.Net;

namespace GOtica.Exceptions.ExceptionsBase;

public class ForbiddenException(string message) : GOticaException(message)
{
    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Forbidden;
}
