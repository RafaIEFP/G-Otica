using GOtica.Exceptions.Resources;
using System.Net;

namespace GOtica.Exceptions.ExceptionsBase;

public class InvalidLoginException() : GOticaException(ResourceMessagesException.EMAIL_OR_PASSWORD_INVALID)
{
    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}
