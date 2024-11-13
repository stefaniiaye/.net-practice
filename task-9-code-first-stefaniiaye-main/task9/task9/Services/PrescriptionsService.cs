using task9.Models;
using task9.Repositories;

namespace task9.Services;

public class PrescriptionsService : IPrescriptionsService
{
    private readonly IPrescriptionsRepo _repo;

    public PrescriptionsService(IPrescriptionsRepo repo)
    {
        _repo = repo;
    }

    public async Task AddNewPrescriptionAsync(NewPrescriptionRequestDTO dto)
    {
        if (dto.DueDate < dto.Date)
        {
            throw new Exception("Prescription due date is expired.");
        }
        
        if (dto.Medicaments.Count() > 10)
        {
            throw new Exception("The number of medicaments is more then 10.");
        }
        
        if (!await _repo.AllMedicamentsExistAsync(dto.Medicaments))
        {
            throw new Exception("Not all medicaments in prescription exist.");
        }
        
        if (!await _repo.PatientExistsAsync(dto.Patient))
        {
            dto.Patient = await _repo.AddNewPatientAsync(dto.Patient);
        }

        await _repo.AddNewPrescriptionAsync(dto);
    }
    
}