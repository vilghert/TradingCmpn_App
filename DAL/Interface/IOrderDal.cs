public interface IOrderDal
{
    Task<List<OrderDto>> GetAllAsync();
    Task<OrderDto> GetByIdAsync(int id);
    Task InsertAsync(OrderDto order);
    void Update(OrderDto order);
    void Delete(int id);
    Task RepeatOrderAsync(int orderId);
    List<OrderDto> GetOrderHistory(int userId);
    List<OrderDto> GetOrdersForUser(int userId);
    List<OrderItemDto> GetOrderItems(int orderId);
}