namespace task5.Models;

public class UpdateRequest
{
    public int OrderID { get; set; }
    public int ProductID { get; set; }
    public int NewQuantity { get; set; }
}