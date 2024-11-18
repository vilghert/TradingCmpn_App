public class ProductDto
{
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public int CategoryId { get; set; }
    public decimal Price { get; set; }
    public int Id
    {
        get => ProductId;
        set => ProductId = value;
    }
}