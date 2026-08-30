namespace GOtica.Communication.Response.Prescription;

public record ResponseGetPrescription
{
    public Guid Id { get; init; }

    public string DoctorName { get; init; } = string.Empty;
    public string DoctorRegistration { get; init; } = string.Empty;

    public DateOnly PrescriptionDate { get; init; }
    public DateOnly ExpirationDate { get; init; }

    public decimal? RightEyeSphere { get; init; }
    public decimal? LeftEyeSphere { get; init; }

    public decimal? RightEyeCylinder { get; init; }
    public decimal? LeftEyeCylinder { get; init; }

    public int? RightEyeAxis { get; init; }
    public int? LeftEyeAxis { get; init; }

    public string? RightEyeVisualAcuity { get; init; }
    public string? LeftEyeVisualAcuity { get; init; }

    public decimal? Addition { get; init; }

    public string? NearVisualAcuity { get; init; }

    public DateOnly? RecommendedReturnDate { get; init; }

    public string? Notes { get; init; }
}
