using GOtica.Exceptions.Resources;
using System.Net;

namespace GOtica.Exceptions.ExceptionsBase;

public class UserCannotDeactivateAccountException() : GOticaException(ResourceMessagesException.USER_IS_STILL_OWNER)
{
    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Conflict;
}
