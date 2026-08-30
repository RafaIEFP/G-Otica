namespace GOtica.Domain.Dtos;

public class PrescriptionListDto
{
    public Guid Id { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public string DoctorRegistration { get; set; } = string.Empty;
    public DateOnly PrescriptionDate { get; set; }
    public DateOnly ExpirationDate { get; set; }
}