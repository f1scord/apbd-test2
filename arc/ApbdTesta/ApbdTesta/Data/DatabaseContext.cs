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
            new() { DoctorId = 1, FirstName = "Adam", LastName = "Smith", Specialization = "Cardiology", Phone = "111222333" },
            new() { DoctorId = 2, FirstName = "Eva",  LastName = "Brown", Specialization = "Dermatology", Phone = "444555666" },
        });

        modelBuilder.Entity<MedicalService>().HasData(new List<MedicalService>
        {
            new() { ServiceId = 1, Name = "Consultation", Description = "General consultation", Price = 100, DurationMinutes = 30 },
            new() { ServiceId = 2, Name = "Blood Test",   Description = "Basic blood test",     Price = 50,  DurationMinutes = 15 },
        });

        modelBuilder.Entity<Patient>().HasData(new List<Patient>
        {
            new() { PatientId = 1, FirstName = "John", LastName = "Doe", DateOfBirth = DateTime.Parse("1990-01-01"), Phone = "123456789" },
            new() { PatientId = 2, FirstName = "Jane", LastName = "Doe", DateOfBirth = DateTime.Parse("1985-06-15"), Phone = "987654321" },
        });

        modelBuilder.Entity<Appointment>().HasData(new List<Appointment>
        {
            new() { AppointmentId = 1, PatientId = 1, DoctorId = 1, AppointmentDate = DateTime.Parse("2026-05-20"), Status = "Completed" },
            new() { AppointmentId = 2, PatientId = 2, DoctorId = 2, AppointmentDate = DateTime.Parse("2026-05-21"), Status = "Completed" },
        });

        modelBuilder.Entity<AppointmentService>().HasData(new List<AppointmentService>
        {
            new() { AppointmentId = 1, ServiceId = 1, Quantity = 1, PerformedAt = DateTime.Parse("2026-05-20") },
            new() { AppointmentId = 2, ServiceId = 2, Quantity = 1, PerformedAt = DateTime.Parse("2026-05-21") },
        });
    }
}
