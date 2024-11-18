using BusinessLogic.Interface;

public class ReviewService : IReviewService
{
    private readonly IReviewDal _reviewDal;

    public ReviewService(IReviewDal reviewDal)
    {
        _reviewDal = reviewDal;
    }

    public async Task<List<ReviewDto>> GetAllReviewsAsync()
    {
        return await _reviewDal.GetAllAsync();
    }

    public async Task<ReviewDto> GetReviewByIdAsync(int id)
    {
        return await _reviewDal.GetByIdAsync(id);
    }

    public async Task<int> CreateReviewAsync(ReviewDto review)
    {
        return await _reviewDal.InsertAsync(review);
    }

    public async Task UpdateReviewAsync(ReviewDto review)
    {
        await _reviewDal.UpdateAsync(review);
    }

    public async Task DeleteReviewAsync(int id)
    {
        await _reviewDal.DeleteAsync(id);
    }

    public async Task<List<ReviewDto>> GetReviewsForProductAsync(int productId)
    {
        return await _reviewDal.GetReviewsForProductAsync(productId);
    }

    public async Task AddReviewAsync(int productId, int userId, string reviewText)
    {
        var review = new ReviewDto
        {
            ProductId = productId,
            UserId = userId,
            ReviewText = reviewText
        };

        await _reviewDal.InsertAsync(review);
    }
}
