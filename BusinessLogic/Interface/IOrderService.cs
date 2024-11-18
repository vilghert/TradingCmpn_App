public interface IOrderService
{
    Task<List<OrderDto>> GetAllOrdersAsync();
    Task<OrderDto> GetOrderByIdAsync(int id);
    void CreateOrder(OrderDto order);
    void UpdateOrder(OrderDto order);
    void DeleteOrder(int id);
    Task RepeatOrderAsync(int orderId);
    List<OrderDto> GetOrderHistory(int userId);
    Task<List<OrderDto>> SearchOrdersAsync(decimal minTotalAmount, decimal maxTotalAmount);
    Task<List<OrderDto>> SortOrdersAsync(string sortBy);
    List<OrderDto> GetOrdersForCurrentUser();
    List<OrderItemDto> GetOrderItems(int orderId);
}