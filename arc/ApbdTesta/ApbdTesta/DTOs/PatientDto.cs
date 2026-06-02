namespace ApbdTesta.DTOs;

public class PatientDto
{
    public string   FirstName   { get; set; } = string.Empty;
    public string   LastName    { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string   Phone       { get; set; } = string.Empty;
    public List<AppointmentDto> Appointments { get; set; } = [];
}

public class AppointmentDto
{
    public int        AppointmentId   { get; set; }
    public DoctorDto  Doctor          { get; set; } = null!;
    public DateTime   AppointmentDate { get; set; }
    public string     Status          { get; set; } = string.Empty;
    public List<AppointmentServiceDto> AppointmentServices { get; set; } = [];
}

public class DoctorDto
{
    public string FirstName      { get; set; } = string.Empty;
    public string LastName       { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
    public string Phone          { get; set; } = string.Empty;
}

public class AppointmentServiceDto
{
    public int              Quantity       { get; set; }
    public DateTime         PerformedAt    { get; set; }
    public MedicalServiceDto MedicalService { get; set; } = null!;
}

public class MedicalServiceDto
{
    public int     ServiceId       { get; set; }
    public string  Name            { get; set; } = string.Empty;
    public string  Description     { get; set; } = string.Empty;
    public decimal Price           { get; set; }
    public int     DurationMinutes { get; set; }
}
