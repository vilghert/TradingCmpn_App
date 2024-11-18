using DTO;
using System.Collections.Generic;

public interface IOrderStatusService
{
    List<OrderStatusDto> GetAllOrderStatuses();
    OrderStatusDto GetOrderStatusById(int id);
    void CreateOrderStatus(OrderStatusDto orderStatus);
    void UpdateOrderStatus(OrderStatusDto orderStatus);
    void DeleteOrderStatus(int id);
}
