using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.Models
{
    public class Appointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AppointmentId { get; set; }
        public string PatientName { get; set; }
        public string AppointmentIllnes { get; set; }
        public String AvailableDoctor { get; set; }
    }
}
