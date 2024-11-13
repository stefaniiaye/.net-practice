using task9.Entities;
using task9.Models;

namespace task9.Repositories;

public interface IPatientsRepo
{
    Task<PatientInfoDTO> GetPatientInfoAsync(int patientId);
    Task<bool> PatientExistsAsync(int id);
    Task AddNewUser(RegisterRequest request);
    Task<AppUser> GetUserByLoginAsync(string login);
    Task UpdateUserAsync(AppUser user);
    Task<AppUser> GetUserByRefreshTokenAsync(string refreshToken);
}