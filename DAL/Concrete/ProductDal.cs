using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

public class ProductDal : IProductDal
{
    private readonly string _connectionString;

    public ProductDal(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<ProductDto>> GetAllAsync()
    {
        var products = new List<ProductDto>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync(); // Асинхронне відкриття з'єднання
            SqlCommand command = new SqlCommand("SELECT ProductId, ProductName, CategoryId, Price FROM Products", connection);
            using (SqlDataReader reader = await command.ExecuteReaderAsync()) // Асинхронне виконання запиту
            {
                while (await reader.ReadAsync()) // Асинхронний виклик для читання запису
                {
                    var product = new ProductDto
                    {
                        ProductId = reader.GetInt32(0),
                        ProductName = reader.GetString(1),
                        CategoryId = reader.GetInt32(2),
                        Price = reader.GetDecimal(3)
                    };
                    products.Add(product);
                }
            }
        }

        return products;
    }

    public async Task<ProductDto> GetByIdAsync(int id)
    {
        ProductDto product = null;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync(); // Асинхронне відкриття з'єднання
            SqlCommand command = new SqlCommand("SELECT ProductId, ProductName, CategoryId, Price FROM Products WHERE ProductId = @id", connection);
            command.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = await command.ExecuteReaderAsync(); // Асинхронне виконання запиту
            if (await reader.ReadAsync()) // Асинхронне читання запису
            {
                product = new ProductDto
                {
                    ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                    ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                    CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                    Price = reader.GetDecimal(reader.GetOrdinal("Price"))
                };
            }
        }
        return product;
    }

    public async Task InsertAsync(ProductDto product)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync(); // Асинхронне відкриття з'єднання
            SqlCommand command = new SqlCommand("INSERT INTO Products (ProductName, CategoryId, Price) OUTPUT inserted.ProductId VALUES (@ProductName, @CategoryId, @Price)", connection);
            command.Parameters.AddWithValue("@ProductName", product.ProductName);
            command.Parameters.AddWithValue("@CategoryId", product.CategoryId);
            command.Parameters.AddWithValue("@Price", product.Price);

            product.ProductId = Convert.ToInt32(await command.ExecuteScalarAsync()); // Асинхронний виконання запиту для отримання значення
        }
    }

    public async Task UpdateAsync(ProductDto product)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync(); // Асинхронне відкриття з'єднання
            SqlCommand command = new SqlCommand("UPDATE Products SET ProductName = @ProductName, CategoryId = @CategoryId, Price = @Price WHERE ProductId = @ProductId", connection);
            command.Parameters.AddWithValue("@ProductId", product.ProductId);
            command.Parameters.AddWithValue("@ProductName", product.ProductName);
            command.Parameters.AddWithValue("@CategoryId", product.CategoryId);
            command.Parameters.AddWithValue("@Price", product.Price);
            await command.ExecuteNonQueryAsync(); // Асинхронне виконання запиту
        }
    }

    public async Task DeleteAsync(int productId)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync(); // Асинхронне відкриття з'єднання
            SqlCommand command = new SqlCommand("DELETE FROM Products WHERE ProductId = @ProductId", connection);
            command.Parameters.AddWithValue("@ProductId", productId);
            await command.ExecuteNonQueryAsync(); // Асинхронне виконання запиту
        }
    }
}
