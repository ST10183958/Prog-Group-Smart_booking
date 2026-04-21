using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.Models
{
    public class Appointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AppointmentId { get; set; }

        public string Province { get; set; }

        public string Surburb { get; set; }

        public int AppointmentSession { get; set; }

        public string AppointmentIllness { get; set; }

        public DateTime PreferredAppointmentDate { get; set; }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }

        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; }
    }
}