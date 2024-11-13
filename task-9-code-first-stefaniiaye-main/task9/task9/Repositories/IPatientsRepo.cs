using task9.Models;

namespace task9.Repositories;

public interface IPatientsRepo
{
    Task<PatientInfoDTO> GetPatientInfoAsync(int patientId);
    Task<bool> PatientExistsAsync(int id);
}