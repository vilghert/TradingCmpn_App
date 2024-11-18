using DTO;

public class OrderStatusService : IOrderStatusService
{
    private readonly IOrderStatusDal _orderStatusDal;

    public OrderStatusService(IOrderStatusDal orderStatusDal)
    {
        _orderStatusDal = orderStatusDal;
    }

    public List<OrderStatusDto> GetAllOrderStatuses()
    {
        return _orderStatusDal.GetAll();
    }

    public OrderStatusDto GetOrderStatusById(int id)
    {
        return _orderStatusDal.GetById(id);
    }

    public void CreateOrderStatus(OrderStatusDto orderStatus)
    {
        _orderStatusDal.Insert(orderStatus);
    }

    public void UpdateOrderStatus(OrderStatusDto orderStatus)
    {
        _orderStatusDal.Update(orderStatus);
    }

    public void DeleteOrderStatus(int id)
    {
        _orderStatusDal.Delete(id);
    }
}