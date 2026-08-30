namespace GOtica.Domain.Entities;

public class Prescription
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string DoctorName { get; set; } = string.Empty;
    public string DoctorRegistration { get; set; } = string.Empty;
    public DateOnly PrescriptionDate { get; set; }
    public DateOnly ExpirationDate { get; set; }
    public decimal? RightEyeSphere { get; set; }
    public decimal? LeftEyeSphere { get; set; }
    public decimal? RightEyeCylinder { get; set; }
    public decimal? LeftEyeCylinder { get; set; }
    public int? RightEyeAxis { get; set; }
    public int? LeftEyeAxis { get; set; }
    public string? RightEyeVisualAcuity { get; set; }
    public string? LeftEyeVisualAcuity { get; set; }
    public decimal? Addition { get; set; }
    public string? NearVisualAcuity { get; set; }
    public DateOnly? RecommendedReturnDate { get; set; }
    public string? Notes { get; set; }

    public Guid ClientId { get; set; }
    public Client Client { get; set; } = default!;
}
