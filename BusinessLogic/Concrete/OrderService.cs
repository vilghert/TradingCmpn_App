public class OrderService : IOrderService
{
    private readonly IOrderDal _orderDal;

    public OrderService(IOrderDal orderDal)
    {
        _orderDal = orderDal;
    }

    public async Task<List<OrderDto>> GetAllOrdersAsync()
    {
        return await _orderDal.GetAllAsync();
    }

    public async Task<OrderDto> GetOrderByIdAsync(int id)
    {
        return await _orderDal.GetByIdAsync(id);
    }

    public void CreateOrder(OrderDto order)
    {
        _orderDal.InsertAsync(order);
    }

    public void UpdateOrder(OrderDto order)
    {
        _orderDal.Update(order);
    }

    public void DeleteOrder(int id)
    {
        _orderDal.Delete(id);
    }

    public List<OrderDto> GetOrderHistory(int userId)
    {
        return _orderDal.GetOrderHistory(userId);
    }

    public async Task<List<OrderDto>> SearchOrdersAsync(decimal minTotalAmount, decimal maxTotalAmount)
    {
        var orders = await _orderDal.GetAllAsync();
        return orders.Where(o => o.TotalAmount >= minTotalAmount && o.TotalAmount <= maxTotalAmount).ToList();
    }

    public async Task<List<OrderDto>> SortOrdersAsync(string sortBy)
    {
        var orders = await _orderDal.GetAllAsync();

        return sortBy switch
        {
            "Total" => orders.OrderBy(o => o.TotalAmount).ToList(),
            _ => orders
        };
    }

    public List<OrderDto> GetOrdersForCurrentUser()
    {
        var currentUserId = 1;
        return _orderDal.GetOrdersForUser(currentUserId);
    }

    public List<OrderItemDto> GetOrderItems(int orderId)
    {
        return _orderDal.GetOrderItems(orderId);
    }
    public async Task RepeatOrderAsync(int orderId)
    {
        try
        {
            var order = await _orderDal.GetByIdAsync(orderId);
            if (order != null)
            {
                var newOrder = new OrderDto
                {
                    UserId = order.UserId,
                    TotalAmount = order.TotalAmount,
                    OrderStatusId = order.OrderStatusId,
                    OrderDate = DateTime.Now,
                    Status = "Pending"
                };
                await _orderDal.InsertAsync(newOrder);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while repeating order: {ex.Message}");
        }
    }
}