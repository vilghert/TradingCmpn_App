using DTO;
using System.Data.SqlClient;

public class OrderStatusDal : IOrderStatusDal
{
    private readonly string _connectionString;

    public OrderStatusDal(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<OrderStatusDto> GetAll()
    {
        List<OrderStatusDto> orderStatuses = new List<OrderStatusDto>();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM OrderStatuses", connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                orderStatuses.Add(new OrderStatusDto
                {
                    OrderStatusId = reader.GetInt32(0),
                    OrderStatusName = reader.GetString(1)
                });
            }
        }
        return orderStatuses;
    }

    public OrderStatusDto GetById(int id)
    {
        OrderStatusDto orderStatus = null;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM OrderStatuses WHERE OrderStatusId = @id", connection);
            command.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                orderStatus = new OrderStatusDto
                {
                    OrderStatusId = reader.GetInt32(0),
                    OrderStatusName = reader.GetString(1)
                };
            }
        }
        return orderStatus;
    }

    public void Insert(OrderStatusDto orderStatus)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand("INSERT INTO OrderStatuses (OrderStatusName) VALUES (@OrderStatusName)", connection);
            command.Parameters.AddWithValue("@OrderStatusName", orderStatus.OrderStatusName);
            command.ExecuteNonQuery();
        }
    }

    public void Update(OrderStatusDto orderStatus)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand("UPDATE OrderStatuses SET OrderStatusName = @OrderStatusName WHERE OrderStatusId = @OrderStatusId", connection);
            command.Parameters.AddWithValue("@OrderStatusId", orderStatus.OrderStatusId);
            command.Parameters.AddWithValue("@OrderStatusName", orderStatus.OrderStatusName);
            command.ExecuteNonQuery();
        }
    }

    public void Delete(int id)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand("DELETE FROM OrderStatuses WHERE OrderStatusId = @id", connection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }
    }
}
