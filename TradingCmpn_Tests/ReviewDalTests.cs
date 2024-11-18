using NUnit.Framework;
using System.Collections.Generic;

[TestFixture]
public class ReviewDalTests
{
    private readonly string connStr;
    private readonly ReviewDal reviewDal;

    public ReviewDalTests()
    {
        connStr = "Data Source=localhost;Initial Catalog=TradingCompanyVicky_Test;Integrated Security=True;TrustServerCertificate=True;";
        reviewDal = new ReviewDal(connStr);
    }

    [Test]
    public void Insert_ShouldAddNewReview()
    {
        var review = new ReviewDto
        {
            ProductId = 1,
            UserId = 3,
            ReviewText = "Test review"
        };

        reviewDal.InsertAsync(review);

        var reviews = reviewDal.GetAllAsync();
        Assert.That(reviews.Exists(r => r.ReviewText == "Test review"), Is.True);
    }

    [Test]
    public void GetAll_ShouldReturnReviews()
    {
        var reviews = reviewDal.GetAllAsync();

        Assert.That(reviews, Is.Not.Null);
    }

    [Test]
    public void GetById_ShouldReturnReview()
    {
        var review = new ReviewDto
        {
            ProductId = 1,
            UserId = 4,
            ReviewText = "Good review"
        };
        reviewDal.InsertAsync(review);

        var fetchedReview = reviewDal.GetByIdAsync(review.ReviewId);

        Assert.That(fetchedReview, Is.Not.Null);
        Assert.That(fetchedReview.ReviewText, Is.EqualTo(review.ReviewText));
    }

    [Test]
    public void Delete_ShouldRemoveReview()
    {
        var review = new ReviewDto
        {
            ProductId = 1,
            UserId = 3,
            ReviewText = "Tegh review"
        };
        reviewDal.InsertAsync(review);
        var reviewId = review.ReviewId;

        reviewDal.DeleteAsync(reviewId);

        var reviewsAfterDelete = reviewDal.GetAllAsync();
        Assert.That(reviewsAfterDelete.Exists(r => r.ReviewId == reviewId), Is.False);
    }
}