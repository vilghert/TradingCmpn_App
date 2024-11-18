using System.Threading.Tasks;

public interface IAuthService
{
    Task<UserDto?> AuthenticateAsync(string username, string password);
    Task<string> GetRoleNameAsync(int roleID);
    Task<string> GetCurrentUserRoleAsync();
    void Logout();
    string CurrentUserRole { get; }
    UserDto? CurrentUser { get; }
    bool VerifyPassword(string password, byte[] storedPasswordHash, byte[] storedPasswordSalt);
    (byte[] passwordHash, byte[] passwordSalt) CreatePasswordHash(string password);
}
