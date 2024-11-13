using Microsoft.AspNetCore.Mvc;
using task9.Models;
using task9.Services;

namespace task9.Controllers;

[ApiController]
[Route("/api/prescriptions")]
public class PrescriptionsController: ControllerBase
{
    private readonly IPrescriptionsService _service;

    public PrescriptionsController(IPrescriptionsService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> AddNewPrescriptionAsync(NewPrescriptionRequestDTO dto)
    {
        try
        {
            Console.WriteLine("Contoller method called");
            await _service.AddNewPrescriptionAsync(dto);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}