using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.Models
{
    public class Admin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdminId { get; set; }
        
        [Required]
        public string AdminUsername { get; set; }
        
        [Required]
        public string Password { get; set; }
        
        [Required]
        public int AdminPasskey { get; set; }
    }
}
