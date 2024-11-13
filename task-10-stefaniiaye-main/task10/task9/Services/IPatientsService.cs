using task9.Models;

namespace task9.Services;

public interface IPatientsService
{
    Task<PatientInfoDTO> GetPatientInfoAsync(int patientId);
    Task AddNewUser(RegisterRequest request);
    Task<(string accessToken, string refreshToken)> AuthenticateUserAsync(LoginRequest loginRequest);
    Task<(string AccessToken, string RefreshToken)> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest);

}