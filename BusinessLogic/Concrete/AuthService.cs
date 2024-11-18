using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using System.Text;

public class AuthService : IAuthService
{
    private readonly string _connectionString;
    private UserDto? _currentUser;

    public AuthService(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString), "Рядок підключення не може бути порожнім.");
    }

    public async Task<UserDto?> AuthenticateAsync(string username, string password)
    {
        UserDto? user = null;

        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(
                    "SELECT UserID, UserName, Email, PasswordHash, PasswordSalt, CreatedAt, RoleID " +
                    "FROM Users " +
                    "WHERE UserName = @username", connection);

                command.Parameters.Add(new SqlParameter("@username", SqlDbType.NVarChar) { Value = username });

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        var storedPasswordHash = reader["PasswordHash"] as byte[];
                        var storedPasswordSalt = reader["PasswordSalt"] as byte[];

                        if (storedPasswordHash != null && storedPasswordSalt != null)
                        {
                            if (VerifyPassword(password, storedPasswordHash, storedPasswordSalt))
                            {
                                user = new UserDto
                                {
                                    UserId = reader.GetInt32(reader.GetOrdinal("UserID")),
                                    UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    RoleID = reader.GetInt32(reader.GetOrdinal("RoleID")),
                                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                                };

                                user.RoleName = await GetRoleNameAsync(user.RoleID);
                                _currentUser = user;
                            }
                            else
                            {
                                Console.WriteLine("Невірний пароль.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Відсутній хеш пароля або сіль у записі користувача.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Користувача з вказаним ім'ям не знайдено.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка під час автентифікації: {ex.Message}");
        }

        return user;
    }

    public async Task<string> GetRoleNameAsync(int roleID)
    {
        string roleName = string.Empty;

        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("SELECT RoleName FROM Roles WHERE RoleID = @roleID", connection);
                command.Parameters.Add(new SqlParameter("@roleID", SqlDbType.Int) { Value = roleID });

                object result = await command.ExecuteScalarAsync();
                roleName = result?.ToString() ?? string.Empty;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка при отриманні ролі користувача: {ex.Message}");
        }

        return roleName;
    }

    public async Task<string> GetCurrentUserRoleAsync()
    {
        if (_currentUser == null)
        {
            return string.Empty;
        }

        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand("SELECT RoleName FROM Roles WHERE RoleID = @roleID", connection);
                command.Parameters.Add(new SqlParameter("@roleID", SqlDbType.Int) { Value = _currentUser.RoleID });

                object result = await command.ExecuteScalarAsync();
                return result?.ToString() ?? string.Empty;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка при отриманні ролі користувача: {ex.Message}");
            return string.Empty;
        }
    }

    public void Logout()
    {
        _currentUser = null;

        Console.WriteLine("Користувач вийшов з системи.");
    }

    public string CurrentUserRole => _currentUser?.RoleName ?? string.Empty;

    public UserDto? CurrentUser => _currentUser;

    public bool VerifyPassword(string password, byte[] storedPasswordHash, byte[] storedPasswordSalt)
    {
        using (var hmac = new HMACSHA512(storedPasswordSalt))
        {
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(storedPasswordHash);
        }
    }

    public (byte[] passwordHash, byte[] passwordSalt) CreatePasswordHash(string password)
    {
        using (var hmac = new HMACSHA512())
        {
            var passwordSalt = hmac.Key;
            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return (passwordHash, passwordSalt);
        }
    }
}
