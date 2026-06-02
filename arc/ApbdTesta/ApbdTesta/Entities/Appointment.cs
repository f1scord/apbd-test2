using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApbdTesta.Entities;

[Table("Appointments")]
public class Appointment
{
    [Key] public int AppointmentId { get; set; }

    [ForeignKey(nameof(Patient))] public int PatientId { get; set; }
    [ForeignKey(nameof(Doctor))]  public int DoctorId  { get; set; }

    public DateTime AppointmentDate { get; set; }
    [MaxLength(50)] public string Status { get; set; } = null!;

    public Patient Patient { get; set; } = null!;
    public Doctor  Doctor  { get; set; } = null!;

    public ICollection<AppointmentService> AppointmentServices { get; set; } = null!;
}
