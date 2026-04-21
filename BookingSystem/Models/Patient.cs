using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.Models
{
    public class Patient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PatientId { get; set; }

        [Required]
        public string PatientName { get; set; }

        [Required]
        public string PatientSurname { get; set; }

        public string PassportNumber { get; set; }

        public DateTime DateOfBirth { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [NotMapped]
        public string ConfirmPassword { get; set; }

        public string MobileNumber { get; set; }
    }
}