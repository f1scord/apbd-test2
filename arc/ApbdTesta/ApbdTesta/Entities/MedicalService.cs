using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ApbdTesta.Entities;

[Table("Medical_Services")]
public class MedicalService
{
    [Key] public int ServiceId { get; set; }
    [MaxLength(100)] public string Name        { get; set; } = null!;
    [MaxLength(100)] public string Description { get; set; } = null!;
    [Precision(10, 2)] public decimal Price    { get; set; }
    public int DurationMinutes { get; set; }

    public ICollection<AppointmentService> AppointmentServices { get; set; } = null!;
}
