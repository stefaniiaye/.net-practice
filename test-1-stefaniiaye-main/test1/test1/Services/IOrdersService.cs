using task5.Models;

namespace task5.Services;

public interface IOrdersService
{
    Task<IEnumerable<Order>> GetOrdersOfClientAsync(int customerId);
    Task UpdateQuantityInOrderAsync(UpdateRequest request);
}