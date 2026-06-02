using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApbdTesta.Entities;

[Table("Doctors")]
public class Doctor
{
    [Key] public int DoctorId { get; set; }
    [MaxLength(50)]  public string FirstName      { get; set; } = null!;
    [MaxLength(50)]  public string LastName       { get; set; } = null!;
    [MaxLength(100)] public string Specialization { get; set; } = null!;
    [MaxLength(9)]   public string Phone          { get; set; } = null!;

    public ICollection<Appointment> Appointments { get; set; } = null!;
}
