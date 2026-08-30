namespace GOtica.Communication.Response.Prescription;

public record ResponseGetAllPrescription
{
    public Guid Id { get; init; }

    public string DoctorName { get; init; } = string.Empty;
    public string DoctorRegistration { get; init; } = string.Empty;

    public DateOnly PrescriptionDate { get; init; }
    public DateOnly ExpirationDate { get; init; }
}
