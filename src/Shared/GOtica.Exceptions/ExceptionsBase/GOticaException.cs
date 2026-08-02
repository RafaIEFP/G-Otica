using System.Net;

namespace GOtica.Exceptions.ExceptionsBase;

public abstract class GOticaException(string message) : SystemException(message)
{
    public abstract IList<string> GetErrorMessages();
    public abstract HttpStatusCode GetStatusCode();
}
