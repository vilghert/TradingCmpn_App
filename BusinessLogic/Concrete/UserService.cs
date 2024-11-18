using System.Security.Cryptography;
using System.Text;

public class UserService : IUserService
{
    private readonly IUserDal _userDal;
    private readonly IReviewDal _reviewDal;
    private readonly UserContext _userContext;

    public UserService(IUserDal userDal, IReviewDal reviewDal, UserContext userContext)
    {
        _userDal = userDal;
        _reviewDal = reviewDal;
        _userContext = userContext;
    }

    public List<UserDto> GetAllUsers()
    {
        return _userDal.GetAll();
    }

    public UserDto GetUserById(int id)
    {
        return _userDal.GetById(id);
    }

    public void CreateUser(UserDto user, string password, int roleId)
    {
        var (passwordHash, passwordSalt) = CreatePasswordHash(password);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        user.RoleID = roleId;

        user.CreatedAt = DateTime.Now;

        _userDal.Insert(user);
    }

    public void UpdateUser(UserDto user)
    {
        if (user.PasswordHash != null && user.PasswordSalt != null)
        {
            _userDal.Update(user);
        }
        else
        {

            _userDal.Update(new UserDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                RoleID = user.RoleID,
            });
        }
    }

    public void DeleteUser(int id)
    {
        _userDal.Delete(id);
    }

    private (byte[] PasswordHash, byte[] PasswordSalt) CreatePasswordHash(string password)
    {
        using (var hmac = new HMACSHA512())
        {
            return (hmac.ComputeHash(Encoding.UTF8.GetBytes(password)), hmac.Key);
        }
    }

    public void SaveReview(int userId, int productId, string reviewText)
    {
        var review = new ReviewDto
        {
            UserId = userId,
            ProductId = productId,
            ReviewText = reviewText
        };
        _reviewDal.InsertAsync(review);
    }

    public int GetCurrentUserId()
    {
        return _userContext.CurrentUser?.UserId ?? 0;
    }
}