using System.Data.Common;
using System.Data.SqlClient;
using task5.Models;

namespace task5.Services;

public class OrdersService : IOrdersService
{
    private IConfiguration _configuration;
    private readonly string _connectionString;

    public OrdersService(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = configuration.GetConnectionString("DefaultPJATKConnection");
    }

    public async Task<IEnumerable<Order>> GetOrdersOfClientAsync(int customerId)
    {
        var orders = new List<Order>();
        await using var con = new SqlConnection(_connectionString);
        await con.OpenAsync();

        await using var cmd = new SqlCommand();
        cmd.Connection = con;
        
        cmd.CommandText = "SELECT COUNT(*) FROM Customer WHERE CustomerID=@CustomerID";
        cmd.Parameters.AddWithValue("@CustomerID", customerId);
        int customerCount = Convert.ToInt32(await cmd.ExecuteScalarAsync());
        if (customerCount == 0)
        {
            throw new Exception("Customer");
        }
        
        cmd.Parameters.Clear();
        cmd.CommandText = "SELECT * FROM [Order] WHERE CustomerID=@CustomerID";
        cmd.Parameters.AddWithValue("@CustomerID", customerId);
        await using (var dr = await cmd.ExecuteReaderAsync())
        {
            while (dr.Read())
            {
                var o = new Order
                {
                    OrderId = (int)dr["OrderId"],
                    TotalAmount = (decimal)dr["TotalAmount"],
                    OrderDate = (DateTime)dr["OrderDate"]
                };
                orders.Add(o);
            }
        }

        foreach (var order in orders)
        {
            var items = new List<Item>();
            cmd.Parameters.Clear();
            cmd.CommandText = @"SELECT o.Quantity, o.Price, p.Name AS ProductName
                                FROM OrderItem o
                                JOIN Product p ON o.ProductID=p.ProductID
                                WHERE OrderID=@OrderId";
            cmd.Parameters.AddWithValue("@OrderId", order.OrderId);
            
            await using (var dr = await cmd.ExecuteReaderAsync())
            {
                while (dr.Read())
                {
                    var p = new Item
                    {
                        Price = (decimal)dr["Price"],
                        ProductName = dr["ProductName"].ToString(),
                        Quantity = (int)dr["Quantity"]
                    };
                    items.Add(p);
                }
            }

            order.OrderedItems = items;
        }

        return orders;
    }

    public async Task UpdateQuantityInOrderAsync(UpdateRequest request)
    {
        await using var con = new SqlConnection(_connectionString);
        await con.OpenAsync();

        await using var cmd = new SqlCommand();
        cmd.Connection = con;

        DbTransaction tran = await con.BeginTransactionAsync();
        cmd.Transaction = (SqlTransaction)tran;
        
        if (request.NewQuantity <= 0)
        {
            await tran.RollbackAsync();
            throw new Exception("Quantity");
        }

        cmd.CommandText = "SELECT COUNT(*) FROM [Order] WHERE OrderID=@OrderID";
        cmd.Parameters.AddWithValue("@OrderId", request.OrderID);
        int orderCount = Convert.ToInt32(await cmd.ExecuteScalarAsync());
        if (orderCount == 0)
        {
            await tran.RollbackAsync();
            throw new Exception("Order");
        }
        
        cmd.Parameters.Clear();
        cmd.CommandText = "SELECT COUNT(*) FROM Product WHERE ProductID=@ProductID";
        cmd.Parameters.AddWithValue("@ProductId", request.ProductID);
        int productCount = Convert.ToInt32(await cmd.ExecuteScalarAsync());
        if (productCount == 0)
        {
            await tran.RollbackAsync();
            throw new Exception("Product");
        }
        
        cmd.Parameters.Clear();
        cmd.CommandText = "SELECT COUNT(*) FROM OrderItem WHERE OrderID=@OrderID AND ProductID=@ProductID";
        cmd.Parameters.AddWithValue("@ProductId", request.ProductID);
        cmd.Parameters.AddWithValue("@OrderId", request.OrderID);
        int orderItemCount = Convert.ToInt32(await cmd.ExecuteScalarAsync());
        if (orderItemCount == 0)
        {
            await tran.RollbackAsync();
            throw new Exception("OrderItem");
        }
        
        cmd.Parameters.Clear();
        cmd.CommandText =
            "UPDATE OrderItem SET Quantity=@Quantity WHERE OrderID=@OrderID AND ProductID=@ProductID";
        cmd.Parameters.AddWithValue("@ProductId", request.ProductID);
        cmd.Parameters.AddWithValue("@OrderId", request.OrderID);
        cmd.Parameters.AddWithValue("@Quantity", request.NewQuantity);
        await cmd.ExecuteNonQueryAsync();

        await tran.CommitAsync();
    }
}