using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using task9.Models;
using task9.Services;

namespace task9.Controllers;

[ApiController]
[Route("api/patients")]
public class PatientsController : ControllerBase
{
    private readonly IPatientsService _service;

    public PatientsController(IPatientsService service)
    {
        _service = service;
    }

    [HttpGet("{patientId}")]
    public async Task<IActionResult> GetPatientInfoAsync(int patientId)
    {
        var patientInfo = await _service.GetPatientInfoAsync(patientId);
        return Ok(patientInfo);
    }
    
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterStudentAsync(RegisterRequest request)
    {
        await _service.AddNewUser(request);
        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        try
        {
            var (accessToken, refreshToken) = await _service.AuthenticateUserAsync(loginRequest);
            return Ok(new { accessToken, refreshToken });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }
    
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAsync([FromBody] RefreshTokenRequest refreshTokenRequest)
    {
        try
        {
            var tokens = await _service.RefreshTokenAsync(refreshTokenRequest);
            return Ok(new
            {
                accessToken = tokens.AccessToken,
                refreshToken = tokens.RefreshToken
            });
        }
        catch (SecurityTokenException ex)
        {
            return Unauthorized(ex.Message);
        }
    }
}