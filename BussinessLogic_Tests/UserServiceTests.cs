using Moq;
using NUnit.Framework;
using System.Collections.Generic;

[TestFixture]
public class UserServiceTests
{
    private Mock<IUserDal> _userDalMock;
    private Mock<IReviewDal> _reviewDalMock;
    private Mock<UserContext> _userContextMock;
    private UserService _userService;

    [SetUp]
    public void Setup()
    {
        _userDalMock = new Mock<IUserDal>();
        _reviewDalMock = new Mock<IReviewDal>();
        _userContextMock = new Mock<UserContext>();

        _userService = new UserService(_userDalMock.Object, _reviewDalMock.Object, _userContextMock.Object);
    }

    [Test]
    public void GetAllUsers_ReturnsAllUsers()
    {
        var users = new List<UserDto>
        {
            new UserDto { UserId = 1, UserName = "TestUser1", Email = "test1@example.com" },
            new UserDto { UserId = 2, UserName = "TestUser2", Email = "test2@example.com" }
        };

        _userDalMock.Setup(dal => dal.GetAll()).Returns(users);

        var result = _userService.GetAllUsers();

        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("TestUser1", result[0].UserName);
        Assert.AreEqual("TestUser2", result[1].UserName);
    }

    [Test]
    public void GetUserById_ReturnsUser_WhenUserExists()
    {
        var userId = 1;
        var user = new UserDto { UserId = userId, UserName = "TestUser", Email = "test@example.com" };

        _userDalMock.Setup(dal => dal.GetById(userId)).Returns(user);

        var result = _userService.GetUserById(userId);

        Assert.IsNotNull(result);
        Assert.AreEqual(userId, result.UserId);
    }

    [Test]
    public void CreateUser_SetsPasswordHashAndSalt()
    {
        var user = new UserDto { UserId = 1, UserName = "NewUser", Email = "new@example.com" };
        string password = "password";
        int roleId = 1;

        _userService.CreateUser(user, password, roleId);

        _userDalMock.Verify(dal => dal.Insert(It.IsAny<UserDto>()), Times.Once);
        Assert.IsNotNull(user.PasswordHash);
        Assert.IsNotNull(user.PasswordSalt);
    }

    [Test]
    public void UpdateUser_CallsUpdateMethod_WhenPasswordHashAndSaltArePresent()
    {
        var user = new UserDto
        {
            UserId = 1,
            UserName = "UpdatedUser",
            Email = "updated@example.com",
            PasswordHash = new byte[64],
            PasswordSalt = new byte[128]
        };

        _userService.UpdateUser(user);

        _userDalMock.Verify(dal => dal.Update(user), Times.Once);
    }

    [Test]
    public void UpdateUser_CallsUpdateMethod_WithoutPasswordHashAndSalt()
    {
        var user = new UserDto
        {
            UserId = 1,
            UserName = "UpdatedUser",
            Email = "updated@example.com"
        };

        _userService.UpdateUser(user);

        _userDalMock.Verify(dal => dal.Update(It.Is<UserDto>(u => u.UserId == user.UserId && u.UserName == user.UserName && u.Email == user.Email)), Times.Once);
    }

    [Test]
    public void DeleteUser_CallsDeleteMethod()
    {
        var userId = 1;

        _userService.DeleteUser(userId);

        _userDalMock.Verify(dal => dal.Delete(userId), Times.Once);
    }

    [Test]
    public void SaveReview_CallsInsertMethod()
    {
        var userId = 1;
        var productId = 2;
        var reviewText = "Great product!";

        _reviewDalMock.Setup(dal => dal.InsertAsync(It.IsAny<ReviewDto>())).Returns(System.Threading.Tasks.Task.FromResult(0));

        _userService.SaveReview(userId, productId, reviewText);

        _reviewDalMock.Verify(dal => dal.InsertAsync(It.Is<ReviewDto>(r => r.UserId == userId && r.ProductId == productId && r.ReviewText == reviewText)), Times.Once);
    }

}
