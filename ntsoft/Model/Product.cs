namespace ntsoft.Model;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
    public string OwnerUserId { get; set; } = default!;
}

public class ProductCreateRequest
{
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
}

public class ProductUpdateRequest
{
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
}
