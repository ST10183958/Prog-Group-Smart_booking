namespace BookingSystem.Models;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Medicine
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MedicineId { get; set; }
    public string MedicineName { get; set; }
    public int Stock { get; set; }
    
}