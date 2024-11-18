using Moq;

[TestFixture]
public class ReviewServiceTests
{
    private ReviewService _reviewService;
    private Mock<IReviewDal> _reviewDalMock;

    [SetUp]
    public void Setup()
    {
        _reviewDalMock = new Mock<IReviewDal>();
        _reviewService = new ReviewService(_reviewDalMock.Object);
    }

    [Test]
    public async Task GetAllReviews_ReturnsAllReviews()
    {
        var reviews = new List<ReviewDto>
        {
            new ReviewDto { ReviewId = 1, ProductId = 1, UserId = 1, ReviewText = "Great product!" },
            new ReviewDto { ReviewId = 2, ProductId = 1, UserId = 2, ReviewText = "Not bad!" }
        };
        _reviewDalMock.Setup(dal => dal.GetAllAsync()).ReturnsAsync(reviews);

        var result = await _reviewService.GetAllReviewsAsync();

        Assert.AreEqual(2, result.Count);
    }

    [Test]
    public async Task GetReviewById_ReturnsCorrectReview()
    {
        var review = new ReviewDto { ReviewId = 1, ProductId = 1, UserId = 1, ReviewText = "Great product!" };
        _reviewDalMock.Setup(dal => dal.GetByIdAsync(1)).ReturnsAsync(review);

        var result = await _reviewService.GetReviewByIdAsync(1);

        Assert.AreEqual(review, result);
    }

    [Test]
    public async Task UpdateReview_UpdatesReview()
    {
        var existingReview = new ReviewDto { ReviewId = 1, ProductId = 1, UserId = 1, ReviewText = "Updated review!" };

        await _reviewService.UpdateReviewAsync(existingReview);

        _reviewDalMock.Verify(dal => dal.UpdateAsync(existingReview), Times.Once);
    }

    [Test]
    public async Task DeleteReview_DeletesReview()
    {
        int reviewIdToDelete = 1;

        await _reviewService.DeleteReviewAsync(reviewIdToDelete);

        _reviewDalMock.Verify(dal => dal.DeleteAsync(reviewIdToDelete), Times.Once);
    }
}