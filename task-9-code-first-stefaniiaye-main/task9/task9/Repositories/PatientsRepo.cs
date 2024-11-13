using Microsoft.EntityFrameworkCore;
using task9.Context;
using task9.Models;

namespace task9.Repositories;

public class PatientsRepo : IPatientsRepo
{
    private readonly MedicalContext _context;

    public PatientsRepo(MedicalContext context)
    {
        _context = context;
    }

    public async Task<PatientInfoDTO> GetPatientInfoAsync(int patientId)
    {
        var patient = await _context.Patients
            .Include(p => p.Prescriptions)
            .ThenInclude(p => p.Medicaments)
            .ThenInclude(pm => pm.Medicament)
            .Include(p => p.Prescriptions)
            .ThenInclude(p => p.Doctor)
            .FirstOrDefaultAsync(p => p.IdPatient == patientId);
        
        return new PatientInfoDTO
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Birthdate = patient.Birthdate,
            Prescriptions = patient.Prescriptions
                .OrderBy(p => p.DueDate)
                .Select(p => new PrescriptionDTO
                {
                    IdPrescription = p.IdPrescription,
                    Date = p.Date,
                    DueDate = p.DueDate,
                    Medicaments = p.Medicaments.Select(pm => new MedicamentDTOforPatient
                    {
                        IdMedicament = pm.Medicament.IdMedicament,
                        Name = pm.Medicament.Name,
                        Dose = pm.Dose,
                        Description = pm.Medicament.Description
                    }).ToList(),
                    Doctor = new DoctorDTO
                    {
                        IdDoctor = p.Doctor.IdDoctor,
                        FirstName = p.Doctor.FirstName,
                        LastName = p.Doctor.LastName
                    }
                }).ToList()
        };
    }

    public async Task<bool> PatientExistsAsync(int id)
    {
        return await _context.Patients.AnyAsync(x => x.IdPatient == id);
    }
}