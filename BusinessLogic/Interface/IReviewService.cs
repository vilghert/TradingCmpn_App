namespace BusinessLogic.Interface
{
    public interface IReviewService
    {
        Task AddReviewAsync(int productId, int userId, string reviewText);
        Task<List<ReviewDto>> GetAllReviewsAsync();
        Task<ReviewDto> GetReviewByIdAsync(int id);
        Task<int> CreateReviewAsync(ReviewDto review);
        Task UpdateReviewAsync(ReviewDto review);
        Task DeleteReviewAsync(int id);
        Task<List<ReviewDto>> GetReviewsForProductAsync(int productId);
    }
}