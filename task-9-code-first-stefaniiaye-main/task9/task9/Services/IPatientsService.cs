using task9.Models;

namespace task9.Services;

public interface IPatientsService
{
    Task<PatientInfoDTO> GetPatientInfoAsync(int patientId);
    
}