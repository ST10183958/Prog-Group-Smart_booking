using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace BookingSystem.Models
{
    public class Doctor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }  
        public string DoctorSurname { get; set; }
        public string DoctorEmail { get; set; }
        public string DoctorPassword { get; set; }
        public int DoctorNumber { get; set; }
        public int PassportNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        
    }
}
