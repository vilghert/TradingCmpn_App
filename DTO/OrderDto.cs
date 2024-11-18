public class OrderDto
{
    public int OrderId { get; set; }
    public int UserId { get; set; }
    public decimal TotalAmount { get; set; }
    public int OrderStatusId { get; set; }
    public List<OrderItemDto> OrderItems { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public string? Status { get; set; }
}