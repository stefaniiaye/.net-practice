using task9.Models;
using task9.Repositories;

namespace task9.Services;

public class PatientsService : IPatientsService
{
    private readonly IPatientsRepo _repo;

    public PatientsService(IPatientsRepo repo)
    {
        _repo = repo;
    }

    public async Task<PatientInfoDTO> GetPatientInfoAsync(int patientId)
    {
        if (!await _repo.PatientExistsAsync(patientId))
        {
            throw new Exception("Patient doesn't exist.");
        }
        return await _repo.GetPatientInfoAsync(patientId);
    }
}