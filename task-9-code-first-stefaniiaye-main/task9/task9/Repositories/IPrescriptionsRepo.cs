using task9.Models;

namespace task9.Repositories;

public interface IPrescriptionsRepo
{
    Task AddNewPrescriptionAsync(NewPrescriptionRequestDTO dto);
    Task<PatientDTO> AddNewPatientAsync(PatientDTO patientDto);
    Task<bool> PatientExistsAsync(PatientDTO patientDto);
    Task<bool> AllMedicamentsExistAsync(IEnumerable<MedicamentDTO> medicamentDtos);

}