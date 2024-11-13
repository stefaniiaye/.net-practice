using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using task_8.Context;

namespace task_8.Controllers;
[ApiController]
[Route("/api/clients")]
public class ClientsController: ControllerBase
{
    
    private readonly TripsContext _dbContext;

    public ClientsController(TripsContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpDelete("{idClient}")]
    public async Task<IActionResult> DeleteClientAsync(int idClient)
    {
        if (await _dbContext.ClientTrips.AnyAsync(ct => ct.IdClient == idClient))
        {
            return BadRequest("Oops, client cannot be deleted");
        }

        var client = await _dbContext.Clients.FindAsync(idClient);

        if (client == null)
        {
            return NotFound("No clients found");
        }

        _dbContext.Clients.Remove(client);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }
}