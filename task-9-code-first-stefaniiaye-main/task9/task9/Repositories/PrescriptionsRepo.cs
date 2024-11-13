using Microsoft.EntityFrameworkCore;
using task9.Context;
using task9.Entities;
using task9.Models;

namespace task9.Repositories;

public class PrescriptionsRepo : IPrescriptionsRepo
{
    private readonly MedicalContext _context;

    public PrescriptionsRepo(MedicalContext context)
    {
        _context = context;
    }

    public async Task AddNewPrescriptionAsync(NewPrescriptionRequestDTO dto)
    {
        var prescription = new Prescription
        {
            IdPatient = dto.Patient.IdPatient,
            IdDoctor = dto.Doctor.IdDoctor,
            Date = dto.Date,
            DueDate = dto.DueDate,
            Medicaments = new List<Prescription_Medicament>()
        };
        
        foreach (var m in dto.Medicaments)
        {
            var prescriptionMedicament = new Prescription_Medicament
            {
                IdMedicament = m.IdMedicament,
                IdPrescription = prescription.IdPrescription,
                Details = m.Details,
                Dose = m.Dose
            };
            prescription.Medicaments.Add(prescriptionMedicament);
            await _context.PrescriptionsMedicaments.AddAsync(prescriptionMedicament);
        }
        
        await _context.Prescriptions.AddAsync(prescription);
        await _context.SaveChangesAsync();
    }
    
    public async Task<bool> PatientExistsAsync(PatientDTO patientDto)
    {
        return await _context.Patients.AnyAsync(x => x.IdPatient == patientDto.IdPatient &&
                                                  x.FirstName == patientDto.FirstName &&
                                                  x.LastName == patientDto.LastName);
    }

    public async Task<PatientDTO> AddNewPatientAsync(PatientDTO patientDto)
    {
        var patient = new Patient
        {
            FirstName = patientDto.FirstName,
            LastName = patientDto.LastName,
            Birthdate = patientDto.Birthdate
        };
        await _context.Patients.AddAsync(patient);
        await _context.SaveChangesAsync();
        
        var dto = new PatientDTO
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Birthdate = patient.Birthdate
        };
        return dto;
    }

    public async Task<bool> AllMedicamentsExistAsync(IEnumerable<MedicamentDTO> medicamentDtos)
    {
        foreach (var m in medicamentDtos)
        {
            if (!await _context.Medicaments.AnyAsync(x => x.IdMedicament == m.IdMedicament))
            {
                return false;
            }
        }
        return true;
    }
}