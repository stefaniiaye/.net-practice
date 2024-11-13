using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using task9.Helpers;
using task9.Models;
using task9.Repositories;

namespace task9.Services;

public class PatientsService : IPatientsService
{
    private readonly IPatientsRepo _repo;
    private readonly IConfiguration _configuration;

    public PatientsService(IPatientsRepo repo, IConfiguration configuration)
    {
        _repo = repo;
        _configuration = configuration;
    }

    public async Task<PatientInfoDTO> GetPatientInfoAsync(int patientId)
    {
        if (!await _repo.PatientExistsAsync(patientId))
        {
            throw new Exception("Patient doesn't exist.");
        }
        return await _repo.GetPatientInfoAsync(patientId);
    }

    public async Task AddNewUser(RegisterRequest request)
    {
        await _repo.AddNewUser(request);
    }
    
    public async Task<(string accessToken, string refreshToken)> AuthenticateUserAsync(LoginRequest loginRequest)
    {
        var user = await _repo.GetUserByLoginAsync(loginRequest.Login);

        if (user == null)
            throw new UnauthorizedAccessException("Invalid login credentials");

        var curHashedPassword = SecurityHelpers.GetHashedPasswordWithSalt(loginRequest.Password, user.Salt);

        if (user.Password != curHashedPassword)
            throw new UnauthorizedAccessException("Invalid login credentials");

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Login),
            new Claim(ClaimTypes.Role, "user"),
            new Claim(ClaimTypes.Role, "admin")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: creds
        );

        user.RefreshToken = SecurityHelpers.GenerateRefreshToken();
        user.RefreshTokenExp = DateTime.Now.AddDays(1);
        await _repo.UpdateUserAsync(user);

        return (new JwtSecurityTokenHandler().WriteToken(token), user.RefreshToken);
    }
    
    public async Task<(string AccessToken, string RefreshToken)> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest)
    {
        var user = await _repo.GetUserByRefreshTokenAsync(refreshTokenRequest.RefreshToken);
        if (user == null)
        {
            throw new SecurityTokenException("Invalid refresh token");
        }

        if (user.RefreshTokenExp < DateTime.Now)
        {
            throw new SecurityTokenException("Refresh token expired");
        }

        var userClaims = new[]
        {
            new Claim(ClaimTypes.Name, user.Login),
            new Claim(ClaimTypes.Role, "user"),
            new Claim(ClaimTypes.Role, "admin")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var jwtToken = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: userClaims,
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: creds
        );

        user.RefreshToken = SecurityHelpers.GenerateRefreshToken();
        user.RefreshTokenExp = DateTime.Now.AddDays(1);

        await _repo.UpdateUserAsync(user);

        return (
            AccessToken: new JwtSecurityTokenHandler().WriteToken(jwtToken),
            RefreshToken: user.RefreshToken
        );
    }
}