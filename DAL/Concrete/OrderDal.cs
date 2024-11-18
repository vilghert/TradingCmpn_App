using System.Data.SqlClient;

public class OrderDal : IOrderDal
{
    private readonly string _connectionString;
    private readonly IOrderItemDal _orderItemDal;

    public OrderDal(string connectionString, IOrderItemDal orderItemDal)
    {
        _connectionString = connectionString;
        _orderItemDal = orderItemDal;
    }

    public async Task<List<OrderDto>> GetAllAsync()
    {
        List<OrderDto> orders = new List<OrderDto>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            SqlCommand command = new SqlCommand("SELECT * FROM Orders", connection);

            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var order = new OrderDto
                    {
                        OrderId = reader.GetInt32(0),
                        UserId = reader.GetInt32(1),
                        TotalAmount = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2),
                        OrderStatusId = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                        OrderItems = _orderItemDal.GetByOrderId(reader.GetInt32(0))
                    };

                    orders.Add(order);
                }
            }
        }

        return orders;
    }

    public async Task<OrderDto> GetByIdAsync(int id)
    {
        OrderDto order = null;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            SqlCommand command = new SqlCommand("SELECT OrderId, UserId, TotalAmount, OrderStatusId FROM Orders WHERE OrderId = @id", connection);
            command.Parameters.AddWithValue("@id", id);

            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    order = new OrderDto
                    {
                        OrderId = reader.GetInt32(0),
                        UserId = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                        TotalAmount = reader.IsDBNull(2) ? 0.0m : reader.GetDecimal(2),
                        OrderStatusId = reader.IsDBNull(3) ? 0 : reader.GetInt32(3)
                    };
                }
            }
        }
        return order;
    }

    public async Task InsertAsync(OrderDto order)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            SqlCommand command = new SqlCommand("INSERT INTO Orders (UserID, TotalAmount, OrderStatusId) " +
                                                "VALUES (@UserId, @TotalAmount, @OrderStatusId); " +
                                                "SELECT SCOPE_IDENTITY();", connection);

            command.Parameters.AddWithValue("@UserId", order.UserId);
            command.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
            command.Parameters.AddWithValue("@OrderStatusId", order.OrderStatusId);

            int orderId = Convert.ToInt32(await command.ExecuteScalarAsync());

            foreach (var orderItem in order.OrderItems)
            {
                orderItem.OrderId = orderId;
                await _orderItemDal.InsertAsync(orderItem);
            }
        }
    }

    public void Update(OrderDto order)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand("UPDATE Orders SET UserId = @UserId, TotalAmount = @TotalAmount, OrderStatusId = @OrderStatusId WHERE OrderId = @OrderId", connection);
            command.Parameters.AddWithValue("@OrderId", order.OrderId);
            command.Parameters.AddWithValue("@UserId", order.UserId);
            command.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
            command.Parameters.AddWithValue("@OrderStatusId", order.OrderStatusId);
            command.ExecuteNonQuery();

            foreach (var orderItem in order.OrderItems)
            {
                if (orderItem.OrderItemId == 0)
                {
                    orderItem.OrderId = order.OrderId;
                    _orderItemDal.InsertAsync(orderItem);
                }
                else
                {
                    _orderItemDal.Update(orderItem);
                }
            }
        }
    }

    public void Delete(int id)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            _orderItemDal.DeleteByOrderId(id);

            SqlCommand command = new SqlCommand("DELETE FROM Orders WHERE OrderId = @id", connection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }
    }

    public List<OrderDto> GetOrderHistory(int userId)
    {
        List<OrderDto> orders = new List<OrderDto>();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand("SELECT OrderId, UserId, TotalAmount, OrderStatusId FROM Orders WHERE UserId = @userId", connection);
            command.Parameters.AddWithValue("@userId", userId);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                var order = new OrderDto
                {
                    OrderId = reader.GetInt32(0),
                    UserId = reader.GetInt32(1),
                    TotalAmount = reader.GetDecimal(2),
                    OrderStatusId = reader.GetInt32(3)
                };
                order.OrderItems = _orderItemDal.GetByOrderId(order.OrderId);

                orders.Add(order);
            }
        }
        return orders;
    }

    public List<OrderDto> GetOrdersForUser(int userId)
    {
        var orders = new List<OrderDto>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM Orders WHERE UserId = @UserId", connection);
            command.Parameters.AddWithValue("@UserId", userId);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var order = new OrderDto
                    {
                        OrderId = reader.GetInt32(0),
                        UserId = reader.GetInt32(1),
                        TotalAmount = reader.GetDecimal(2)
                    };
                    orders.Add(order);
                }
            }
        }

        return orders;
    }

    public async Task RepeatOrderAsync(int orderId)
    {
        var order = await GetByIdAsync(orderId);
        if (order != null)
        {
            order.OrderId = 0;
            await InsertAsync(order);
        }
    }

    public List<OrderItemDto> GetOrderItems(int orderId)
    {
        return _orderItemDal.GetByOrderId(orderId);
    }
}
