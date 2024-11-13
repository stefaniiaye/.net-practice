using Microsoft.AspNetCore.Mvc;
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
        try
        {
            var patientInfo = await _service.GetPatientInfoAsync(patientId);
            return Ok(patientInfo);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
