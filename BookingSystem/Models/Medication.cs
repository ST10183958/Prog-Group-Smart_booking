using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.Models
{

    [Table("Medicines")]
    public class Medicine
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MedicineId { get; set; }

        [Required]
        public string MedicineName { get; set; }

        [Required]
        public int Stock { get; set; }

        [Required]
        public int Dosage { get; set; }
    }
}