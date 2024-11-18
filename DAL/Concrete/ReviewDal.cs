using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

public class ReviewDal : IReviewDal
{
    private readonly string _connectionString;

    public ReviewDal(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<ReviewDto>> GetAllAsync()
    {
        List<ReviewDto> reviews = new List<ReviewDto>();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            SqlCommand command = new SqlCommand("SELECT ReviewId, ProductId, UserId, ReviewText FROM Reviews", connection);
            SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                reviews.Add(new ReviewDto
                {
                    ReviewId = reader.GetInt32(reader.GetOrdinal("ReviewId")),
                    ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                    ReviewText = reader.GetString(reader.GetOrdinal("ReviewText"))
                });
            }
        }
        return reviews;
    }

    public async Task<ReviewDto> GetByIdAsync(int id)
    {
        ReviewDto review = null;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            SqlCommand command = new SqlCommand("SELECT * FROM Reviews WHERE ReviewId = @id", connection);
            command.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                review = new ReviewDto
                {
                    ReviewId = reader.GetInt32(reader.GetOrdinal("ReviewId")),
                    ProductId = reader.IsDBNull(reader.GetOrdinal("ProductId")) ? default : reader.GetInt32(reader.GetOrdinal("ProductId")),
                    UserId = reader.IsDBNull(reader.GetOrdinal("UserId")) ? default : reader.GetInt32(reader.GetOrdinal("UserId")),
                    ReviewText = reader.IsDBNull(reader.GetOrdinal("ReviewText")) ? null : reader.GetString(reader.GetOrdinal("ReviewText"))
                };
            }
        }
        return review;
    }

    public async Task<int> InsertAsync(ReviewDto review)
    {
        int reviewId;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            SqlCommand command = new SqlCommand("INSERT INTO Reviews (ProductId, UserId, ReviewText) OUTPUT INSERTED.ReviewId VALUES (@ProductId, @UserId, @ReviewText)", connection);
            command.Parameters.AddWithValue("@ProductId", review.ProductId);
            command.Parameters.AddWithValue("@UserId", review.UserId);
            command.Parameters.AddWithValue("@ReviewText", review.ReviewText);

            reviewId = (int)await command.ExecuteScalarAsync();
        }
        review.ReviewId = reviewId;
        return reviewId;
    }

    public async Task UpdateAsync(ReviewDto review)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            SqlCommand command = new SqlCommand("UPDATE Reviews SET ProductId = @ProductId, UserId = @UserId, ReviewText = @ReviewText WHERE ReviewId = @ReviewId", connection);
            command.Parameters.AddWithValue("@ReviewId", review.ReviewId);
            command.Parameters.AddWithValue("@ProductId", review.ProductId);
            command.Parameters.AddWithValue("@UserId", review.UserId);
            command.Parameters.AddWithValue("@ReviewText", review.ReviewText);
            await command.ExecuteNonQueryAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            SqlCommand command = new SqlCommand("DELETE FROM Reviews WHERE ReviewId = @id", connection);
            command.Parameters.AddWithValue("@id", id);
            await command.ExecuteNonQueryAsync();
        }
    }
    public async Task<List<ReviewDto>> GetReviewsForProductAsync(int productId)
    {
        List<ReviewDto> reviews = new List<ReviewDto>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            SqlCommand command = new SqlCommand("SELECT * FROM Reviews WHERE ProductId = @ProductId", connection);
            command.Parameters.AddWithValue("@ProductId", productId);

            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    reviews.Add(new ReviewDto
                    {
                        ReviewId = reader.GetInt32(0),
                        ProductId = reader.GetInt32(1),
                        ReviewText = reader.GetString(2),
                    });
                }
            }
        }

        return reviews;
    }
}