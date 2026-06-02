using ApbdTesta.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApbdTesta.Data;

public class DatabaseContext : DbContext
{
    public DbSet<Patient>            Patients            { get; set; }
    public DbSet<Doctor>             Doctors             { get; set; }
    public DbSet<Appointment>        Appointments        { get; set; }
    public DbSet<MedicalService>     MedicalServices     { get; set; }
    public DbSet<AppointmentService> AppointmentServices { get; set; }

    protected DatabaseContext() { }
    public DatabaseContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Doctor>().HasData(new List<Doctor>
        {
            new() { DoctorId = 1, FirstName = "Adam",  LastName = "Mazur",    Specialization = "Cardiology",   Phone = "111222333" },
            new() { DoctorId = 2, FirstName = "Ewa",   LastName = "Kaczmarek",Specialization = "Dermatology",  Phone = "222333444" },
        });

        modelBuilder.Entity<MedicalService>().HasData(new List<MedicalService>
        {
            new() { ServiceId = 1, Name = "General Consultation", Description = "Basic medical consultation",  Price = 150, DurationMinutes = 30 },
            new() { ServiceId = 2, Name = "Blood Test",           Description = "Complete blood count",        Price = 80,  DurationMinutes = 15 },
            new() { ServiceId = 3, Name = "ECG",                  Description = "Electrocardiogram examination",Price = 120, DurationMinutes = 20 },
            new() { ServiceId = 4, Name = "Skin Examination",     Description = "Dermatological skin check",   Price = 200, DurationMinutes = 40 },
        });

        modelBuilder.Entity<Patient>().HasData(new List<Patient>
        {
            new() { PatientId = 1, FirstName = "Anna", LastName = "Kowalska", DateOfBirth = DateTime.Parse("1990-03-15"), Phone = "123456789" },
            new() { PatientId = 2, FirstName = "Jan",  LastName = "Nowak",    DateOfBirth = DateTime.Parse("1985-07-22"), Phone = "234567891" },
        });

        modelBuilder.Entity<Appointment>().HasData(new List<Appointment>
        {
            new() { AppointmentId = 1, PatientId = 1, DoctorId = 1, AppointmentDate = DateTime.Parse("2026-05-20"), Status = "Completed" },
            new() { AppointmentId = 2, PatientId = 2, DoctorId = 2, AppointmentDate = DateTime.Parse("2026-05-21"), Status = "Completed" },
        });

        modelBuilder.Entity<AppointmentService>().HasData(new List<AppointmentService>
        {
            new() { AppointmentId = 1, ServiceId = 1, Quantity = 1, PerformedAt = DateTime.Parse("2026-05-20") },
            new() { AppointmentId = 1, ServiceId = 3, Quantity = 1, PerformedAt = DateTime.Parse("2026-05-20") },
            new() { AppointmentId = 2, ServiceId = 4, Quantity = 1, PerformedAt = DateTime.Parse("2026-05-21") },
        });
    }
}
