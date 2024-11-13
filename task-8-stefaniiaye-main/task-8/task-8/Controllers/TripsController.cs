using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using task_8.DTOs;
using task_8.Context;
using task_8.Entities;

namespace task_8.Controllers;

[ApiController]
[Route("/api/trips")]
public class TripsController : ControllerBase
{
    private readonly TripsContext _dbContext;

    public TripsController(TripsContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetTripsAsync(int page = 1, int pageSize = 10)
    {
        var totalCount = await _dbContext.Trips.CountAsync();
        var trips = await _dbContext.Trips
            .OrderByDescending(t => t.DateFrom)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(t => t.ClientTrips)
            .ThenInclude(ct => ct.IdClientNavigation)
            .Include(t => t.IdCountries)
            .Select(t => new TripDTO
            {
                Name = t.Name,
                Description = t.Description,
                DateFrom = t.DateFrom,
                DateTo = t.DateTo,
                MaxPeople = t.MaxPeople,
                Countries = t.IdCountries.Select(c => new CountryDTO { Name = c.Name }).ToList(),
                Clients = t.ClientTrips.Select(ct => new ClientDTO
                {
                    FirstName = ct.IdClientNavigation.FirstName,
                    LastName = ct.IdClientNavigation.LastName
                }).ToList()
            }).ToListAsync();

        var result = new
        {
            pageNum = page,
            pageSize,
            allPages = (int)Math.Ceiling((double)totalCount / pageSize),
            trips
        };

        return Ok(result);
    }
    
    [HttpPost("/{idTrip}/clients")]
    public async Task<ActionResult> AssignToTripAsync(int idTrip, ClientRequest request)
    {
        var trip = await _dbContext.Trips.FindAsync(idTrip);
        if (trip == null || trip.DateFrom <= DateTime.UtcNow)
        {
            return BadRequest("Oops, cannot assign a client to this trip.");
        }
        
        var foundClient = await _dbContext.Clients.AnyAsync(c => c.Pesel == request.Pesel);
        if (foundClient)
        {
            return BadRequest("Client with this PESEL already in the system.");
        }

        var clientAssignedToTrip =
            await _dbContext.ClientTrips.AnyAsync(tc =>
                tc.IdClientNavigation.Pesel == request.Pesel && tc.IdTripNavigation.IdTrip == idTrip);
        if (clientAssignedToTrip)
        {
            return BadRequest("This client is already registered for this trip");
        }

        var client = new Client
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Telephone = request.Telephone,
            Pesel = request.Pesel,
            ClientTrips = new List<ClientTrip>
            {
                new ClientTrip
                {
                    IdTrip = idTrip,
                    RegisteredAt = DateTime.UtcNow,
                    PaymentDate = request.PaymentDate
                }
            }
        };
        _dbContext.Clients.Add(client);
        await _dbContext.SaveChangesAsync();
        return Ok();
    }

}