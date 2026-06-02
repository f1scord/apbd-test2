using ApbdTesta.Data;
using ApbdTesta.DTOs;
using ApbdTesta.Entities;
using ApbdTesta.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ApbdTesta.Services;

public class DbService(DatabaseContext context) : IDbService
{
    public async Task<List<PatientDto>> GetPatientsAsync(string? lastName)
    {
        var query = context.Patients
            .Include(p => p.Appointments)
                .ThenInclude(a => a.Doctor)
            .Include(p => p.Appointments)
                .ThenInclude(a => a.AppointmentServices)
                    .ThenInclude(s => s.MedicalService)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(lastName))
            query = query.Where(p => p.LastName.Contains(lastName));

        var patients = await query.ToListAsync();

        return patients.Select(p => new PatientDto
        {
            FirstName   = p.FirstName,
            LastName    = p.LastName,
            DateOfBirth = p.DateOfBirth,
            Phone       = p.Phone,
            Appointments = p.Appointments.Select(a => new AppointmentDto
            {
                AppointmentId   = a.AppointmentId,
                AppointmentDate = a.AppointmentDate,
                Status          = a.Status,
                Doctor = new DoctorDto
                {
                    FirstName      = a.Doctor.FirstName,
                    LastName       = a.Doctor.LastName,
                    Specialization = a.Doctor.Specialization,
                    Phone          = a.Doctor.Phone,
                },
                AppointmentServices = a.AppointmentServices.Select(s => new AppointmentServiceDto
                {
                    Quantity    = s.Quantity,
                    PerformedAt = s.PerformedAt,
                    MedicalService = new MedicalServiceDto
                    {
                        ServiceId       = s.MedicalService.ServiceId,
                        Name            = s.MedicalService.Name,
                        Description     = s.MedicalService.Description,
                        Price           = s.MedicalService.Price,
                        DurationMinutes = s.MedicalService.DurationMinutes,
                    }
                }).ToList()
            }).ToList()
        }).ToList();
    }

    public async Task CreatePatientAsync(CreatePatientRequest request)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            // Doctor must exist
            var doctor = await context.Doctors
                .FirstOrDefaultAsync(d => d.DoctorId == request.Appointment.DoctorId);
            if (doctor == null)
                throw new NotFoundException($"Doctor with id {request.Appointment.DoctorId} not found.");

            // Appointment date must not be in the past
            if (request.Appointment.AppointmentDate < DateTime.Now)
                throw new BadRequestException("Appointment date cannot be in the past.");

            var patient = new Patient
            {
                FirstName   = request.FirstName,
                LastName    = request.LastName,
                DateOfBirth = request.DateOfBirth,
                Phone       = request.Phone,
            };
            context.Patients.Add(patient);

            var appointment = new Appointment
            {
                Patient         = patient,
                Doctor          = doctor,
                AppointmentDate = request.Appointment.AppointmentDate,
                Status          = "Scheduled",
            };
            context.Appointments.Add(appointment);

            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
