public class ReviewDto
{
    public int ReviewId { get; set; }
    public int ProductId { get; set; }
    public int UserId { get; set; }
    public string? ReviewText { get; set; }
    public string? ProductName { get; set; }
}