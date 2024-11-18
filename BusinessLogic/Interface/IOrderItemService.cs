using System.Collections.Generic;

public interface IOrderItemService
{
    List<OrderItemDto> GetAllOrderItems();
    List<OrderItemDto> GetOrderItemsByOrderId(int orderId);
    void CreateOrderItem(OrderItemDto orderItem);
    void UpdateOrderItem(OrderItemDto orderItem);
    void DeleteOrderItemsByOrderId(int orderId);
}