public class OrderItemService : IOrderItemService
{
    private readonly IOrderItemDal _orderItemDal;

    public OrderItemService(IOrderItemDal orderItemDal)
    {
        _orderItemDal = orderItemDal;
    }

    public List<OrderItemDto> GetAllOrderItems()
    {
        return _orderItemDal.GetAll();
    }

    public List<OrderItemDto> GetOrderItemsByOrderId(int orderId)
    {
        return _orderItemDal.GetByOrderId(orderId);
    }

    public void CreateOrderItem(OrderItemDto orderItem)
    {
        _orderItemDal.InsertAsync(orderItem);
    }

    public void UpdateOrderItem(OrderItemDto orderItem)
    {
        _orderItemDal.Update(orderItem);
    }

    public void DeleteOrderItemsByOrderId(int orderId)
    {
        _orderItemDal.DeleteByOrderId(orderId);
    }
}