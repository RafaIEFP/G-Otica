using GOtica.Exceptions.Resources;
using System.Net;

namespace GOtica.Exceptions.ExceptionsBase;

public class RefreshTokenNotFoundException : GOticaException
{
    public RefreshTokenNotFoundException() : base(ResourceMessagesException.INVALID_SESSION) { }

    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}
