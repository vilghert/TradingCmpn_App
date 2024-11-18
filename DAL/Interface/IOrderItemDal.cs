using System.Collections.Generic;

public interface IOrderItemDal
{
    List<OrderItemDto> GetAll();
    List<OrderItemDto> GetByOrderId(int orderId);
    Task InsertAsync(OrderItemDto orderItem);
    void Update(OrderItemDto orderItem);
    void DeleteByOrderId(int orderId);
}