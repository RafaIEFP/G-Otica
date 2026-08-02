namespace GOtica.Communication.Response;

public record ResponseError
{
    public IList<string> Errors { get; init; } = [];

    public ResponseError(IList<string> errors) => Errors = errors;
    public ResponseError(string error) => Errors.Add(error);
}
