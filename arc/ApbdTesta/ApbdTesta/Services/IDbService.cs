using ApbdTesta.DTOs;

namespace ApbdTesta.Services;

public interface IDbService
{
    Task<List<PatientDto>> GetPatientsAsync(string? lastName);
    Task CreatePatientAsync(CreatePatientRequest request);
}
