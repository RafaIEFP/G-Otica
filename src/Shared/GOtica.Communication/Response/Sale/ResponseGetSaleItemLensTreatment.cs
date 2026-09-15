namespace GOtica.Communication.Response.Sale;

public record ResponseGetSaleItemLensTreatment
{
    public Guid TreatmentId { get; init; }
    public string TreatmentName { get; init; } = string.Empty;
    public decimal UnitPrice { get; init; }
}
