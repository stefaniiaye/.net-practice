using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using task5.Models;
using task5.Services;

namespace task5.Controllers;

[ApiController]
[Route("/api/orders")]
public class OrdersController : ControllerBase
{
    private IOrdersService _service;

    public OrdersController(IOrdersService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrdersOfClientAsync(int customerId)
    {
        try
        {
            var res = await _service.GetOrdersOfClientAsync(customerId);
            return Ok(res);
        }
        catch (Exception e)
        {
            if (e.Message.Equals("Customer"))
            {
                return NotFound("Customer with such ID doesn't exists.");
            }
            else
            {
                return BadRequest(e.Message);
            }
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateQuantityInOrderAsync(UpdateRequest request)
    {
        try
        {
            await _service.UpdateQuantityInOrderAsync(request);
            return Ok("Quantity was updated");
        }
        catch (Exception e)
        {
            if (e.Message.Equals("Quantity"))
            {
                return BadRequest("Quantity should be greater then zero.");
            }
            else if (e.Message.Equals("Order"))
            {
                return NotFound("Order with such ID doesn't exists.");
            }
            else if (e.Message.Equals("Product"))
            {
                return BadRequest("Product with such ID doesn't exists.");
            }
            else if (e.Message.Equals("OrderItem"))
            {
                return BadRequest("Product is not present in the specified order.");
            }
            else
            {
                return BadRequest(e.Message);
            }
        }
        
    }
}