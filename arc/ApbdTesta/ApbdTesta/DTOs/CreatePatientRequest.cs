using System.ComponentModel.DataAnnotations;

namespace ApbdTesta.DTOs;

public class CreatePatientRequest
{
    [Required] [MaxLength(50)]  public string FirstName   { get; set; } = null!;
    [Required] [MaxLength(100)] public string LastName    { get; set; } = null!;
    [Required] public DateTime DateOfBirth { get; set; }
    [Required] [MaxLength(9)]  public string Phone       { get; set; } = null!;
    [Required] public CreateAppointmentDto Appointment { get; set; } = null!;
}

public class CreateAppointmentDto
{
    [Required] public int      DoctorId        { get; set; }
    [Required] public DateTime AppointmentDate { get; set; }
}
