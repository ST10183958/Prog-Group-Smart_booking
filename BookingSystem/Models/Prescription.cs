using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.Models
{
    [Table("Prescription")]
    public class Prescription
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PrescriptionId { get; set; }

        public int? AppointmentId { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public string MedicineName { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public double Dosage { get; set; }

        [ForeignKey("AppointmentId")]
        public Appointment Appointment { get; set; }

        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; }

        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }
    }
}