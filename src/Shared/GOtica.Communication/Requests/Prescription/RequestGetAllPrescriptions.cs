namespace GOtica.Communication.Requests.Prescription;

public record RequestGetAllPrescriptions
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}
