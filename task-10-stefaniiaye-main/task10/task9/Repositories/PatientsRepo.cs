using Microsoft.EntityFrameworkCore;
using task9.Context;
using task9.Entities;
using task9.Helpers;
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

    public async Task AddNewUser(RegisterRequest request)
    {
        var hashedPasswordAndSalt = SecurityHelpers.GetHashedPasswordAndSalt(request.Password);

        var user = new AppUser()
        {
            Login = request.Login,
            Password = hashedPasswordAndSalt.Item1,
            Salt = hashedPasswordAndSalt.Item2,
            RefreshToken = SecurityHelpers.GenerateRefreshToken(),
            RefreshTokenExp = DateTime.Now.AddDays(1)
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
    
    public async Task<AppUser> GetUserByLoginAsync(string login)
    {
        return await _context.Users.Where(u => u.Login == login).FirstOrDefaultAsync();
    }
    
    public async Task<AppUser> GetUserByRefreshTokenAsync(string refreshToken)
    {
        return await _context.Users.Where(u => u.RefreshToken == refreshToken).FirstOrDefaultAsync();
    }

    public async Task UpdateUserAsync(AppUser user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}