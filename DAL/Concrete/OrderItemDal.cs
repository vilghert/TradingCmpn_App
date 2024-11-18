using System.Collections.Generic;
using System.Data.SqlClient;

public class OrderItemDal : IOrderItemDal
{
    private readonly string _connectionString;

    public OrderItemDal(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<OrderItemDto> GetAll()
    {
        List<OrderItemDto> orderItems = new List<OrderItemDto>();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM OrderItems", connection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                var orderItem = new OrderItemDto
                {
                    OrderItemId = reader.GetInt32(0),
                    OrderId = reader.GetInt32(1),
                    ProductId = reader.GetInt32(2),
                    Quantity = reader.GetInt32(3)
                };
                orderItems.Add(orderItem);
            }
        }
        return orderItems;
    }

    public List<OrderItemDto> GetByOrderId(int orderId)
    {
        List<OrderItemDto> orderItems = new List<OrderItemDto>();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM OrderItems WHERE OrderId = @orderId", connection);
            command.Parameters.AddWithValue("@orderId", orderId);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                orderItems.Add(new OrderItemDto
                {
                    OrderItemId = reader.GetInt32(0),
                    OrderId = reader.GetInt32(1),
                    ProductId = reader.GetInt32(2),
                    Quantity = reader.GetInt32(3)
                });
            }
        }
        return orderItems;
    }

    public async Task InsertAsync(OrderItemDto orderItem)  // Асинхронна реалізація
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();  // Асинхронне відкриття з'єднання
            SqlCommand command = new SqlCommand("INSERT INTO OrderItems (OrderId, ProductId, Quantity) VALUES (@OrderId, @ProductId, @Quantity)", connection);
            command.Parameters.AddWithValue("@OrderId", orderItem.OrderId);
            command.Parameters.AddWithValue("@ProductId", orderItem.ProductId);
            command.Parameters.AddWithValue("@Quantity", orderItem.Quantity);
            await command.ExecuteNonQueryAsync();  // Асинхронне виконання запиту
        }
    }

    public void Update(OrderItemDto orderItem)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand("UPDATE OrderItems SET OrderId = @OrderId, ProductId = @ProductId, Quantity = @Quantity WHERE OrderItemId = @OrderItemId", connection);
            command.Parameters.AddWithValue("@OrderItemId", orderItem.OrderItemId);
            command.Parameters.AddWithValue("@OrderId", orderItem.OrderId);
            command.Parameters.AddWithValue("@ProductId", orderItem.ProductId);
            command.Parameters.AddWithValue("@Quantity", orderItem.Quantity);
            command.ExecuteNonQuery();
        }
    }

    public void DeleteByOrderId(int orderId)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand("DELETE FROM OrderItems WHERE OrderId = @orderId", connection);
            command.Parameters.AddWithValue("@orderId", orderId);
            command.ExecuteNonQuery();
        }
    }
}