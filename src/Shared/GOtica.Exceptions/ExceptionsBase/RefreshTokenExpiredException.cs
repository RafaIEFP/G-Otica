using GOtica.Exceptions.Resources;
using System.Net;

namespace GOtica.Exceptions.ExceptionsBase;

public class RefreshTokenExpiredException : GOticaException
{
    public RefreshTokenExpiredException() : base(ResourceMessagesException.EXPIRED_SESSION) { }

    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Forbidden;
}
