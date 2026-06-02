using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApbdTesta.Entities;

[Table("Patients")]
public class Patient
{
    [Key] public int PatientId { get; set; }
    [MaxLength(50)]  public string FirstName   { get; set; } = null!;
    [MaxLength(100)] public string LastName    { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    [MaxLength(9)]   public string Phone       { get; set; } = null!;

    public ICollection<Appointment> Appointments { get; set; } = null!;
}
