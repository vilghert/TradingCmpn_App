using System.Collections.Generic;
using System.Threading.Tasks;

public interface IReviewDal
{
    Task<List<ReviewDto>> GetAllAsync();
    Task<ReviewDto> GetByIdAsync(int id);
    Task<int> InsertAsync(ReviewDto review);
    Task UpdateAsync(ReviewDto review);
    Task DeleteAsync(int id);
    Task<List<ReviewDto>> GetReviewsForProductAsync(int productId);
}