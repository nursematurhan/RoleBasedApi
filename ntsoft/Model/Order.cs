namespace ntsoft.Model;

public class Order
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public string CustomerUserId { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class OrderCreateRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
