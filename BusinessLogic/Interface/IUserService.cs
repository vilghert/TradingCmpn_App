public interface IUserService
{
    List<UserDto> GetAllUsers();
    UserDto GetUserById(int id);
    void CreateUser(UserDto user, string password, int roleId);
    void UpdateUser(UserDto user);
    void DeleteUser(int id);
    void SaveReview(int userId, int productId, string reviewText);
    int GetCurrentUserId();
}