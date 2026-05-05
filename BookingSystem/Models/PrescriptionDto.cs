namespace BookingSystem.Models;

public class PrescriptionDto
{
    public string DoctorName { get; set; }
    public string PatientName { get; set; }
    public string MedicineName { get; set; }
    public int Quantity { get; set; }
}