using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ApbdTesta.Entities;

[Table("Appointment_Services")]
[PrimaryKey(nameof(AppointmentId), nameof(ServiceId))]
public class AppointmentService
{
    [ForeignKey(nameof(Appointment))]    public int AppointmentId { get; set; }
    [ForeignKey(nameof(MedicalService))] public int ServiceId     { get; set; }

    public int      Quantity    { get; set; }
    public DateTime PerformedAt { get; set; }

    public Appointment    Appointment    { get; set; } = null!;
    public MedicalService MedicalService { get; set; } = null!;
}
