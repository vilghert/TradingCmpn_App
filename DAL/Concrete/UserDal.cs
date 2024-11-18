using System.Data;
using System.Data.SqlClient;

public class UserDal : IUserDal
{
    private readonly string _connectionString;

    public UserDal(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<UserDto> GetAll()
    {
        List<UserDto> users = new List<UserDto>();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand(
                "SELECT UserID, UserName, Email, PasswordHash, PasswordSalt, CreatedAt, RoleID " +
                "FROM Users", connection);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    users.Add(new UserDto
                    {
                        UserId = reader.GetInt32(0),
                        UserName = reader.GetString(1),
                        Email = reader.GetString(2),
                        PasswordHash = reader["PasswordHash"] as byte[],
                        PasswordSalt = reader["PasswordSalt"] as byte[],
                        CreatedAt = reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5),
                        RoleID = reader.GetInt32(6),
                        RoleName = ""
                    });
                }
            }
        }
        return users;
    }

    public UserDto GetById(int id)
    {
        UserDto user = null;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand(
                "SELECT UserID, UserName, Email, PasswordHash, PasswordSalt, CreatedAt, RoleID " +
                "FROM Users " +
                "WHERE UserID = @id", connection);
            command.Parameters.AddWithValue("@id", id);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    user = new UserDto
                    {
                        UserId = reader.GetInt32(0),
                        UserName = reader.GetString(1),
                        Email = reader.GetString(2),
                        PasswordHash = reader["PasswordHash"] as byte[],
                        PasswordSalt = reader["PasswordSalt"] as byte[],
                        CreatedAt = reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5),
                        RoleID = reader.GetInt32(6),
                        RoleName = ""
                    };
                }
            }
        }
        return user;
    }

    public void Insert(UserDto user)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            SqlCommand checkCommand = new SqlCommand(
                "SELECT COUNT(*) FROM Users WHERE UserName = @UserName OR Email = @Email", connection);
            checkCommand.Parameters.AddWithValue("@UserName", user.UserName);
            checkCommand.Parameters.AddWithValue("@Email", user.Email);
            int userCount = (int)checkCommand.ExecuteScalar();

            if (userCount > 0)
            {
                throw new InvalidOperationException("Користувач з таким ім'ям або email вже існує.");
            }

            SqlCommand command = new SqlCommand(
                "INSERT INTO Users (UserName, Email, PasswordHash, PasswordSalt, CreatedAt, RoleID) " +
                "VALUES (@UserName, @Email, @PasswordHash, @PasswordSalt, @CreatedAt, @RoleID)",
                connection);

            command.Parameters.AddWithValue("@UserName", user.UserName);
            command.Parameters.AddWithValue("@Email", user.Email);

            command.Parameters.Add("@PasswordHash", SqlDbType.VarBinary, -1).Value = user.PasswordHash;
            command.Parameters.Add("@PasswordSalt", SqlDbType.VarBinary, -1).Value = user.PasswordSalt;

            command.Parameters.AddWithValue("@CreatedAt", user.CreatedAt ?? DateTime.Now);
            command.Parameters.AddWithValue("@RoleID", user.RoleID);

            command.ExecuteNonQuery();
        }
    }


    public void Update(UserDto user)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand(
                "UPDATE Users SET UserName = @UserName, Email = @Email, PasswordHash = @PasswordHash, PasswordSalt = @PasswordSalt, RoleID = @RoleID WHERE UserID = @UserId",
                connection
            );
            command.Parameters.AddWithValue("@UserId", user.UserId);
            command.Parameters.AddWithValue("@UserName", user.UserName);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
            command.Parameters.AddWithValue("@PasswordSalt", user.PasswordSalt);
            command.Parameters.AddWithValue("@RoleID", user.RoleID);
            command.ExecuteNonQuery();
        }
    }

    public void Delete(int id)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            SqlCommand checkCommand = new SqlCommand("SELECT COUNT(*) FROM Users WHERE UserID = @id", connection);
            checkCommand.Parameters.AddWithValue("@id", id);
            int userCount = (int)checkCommand.ExecuteScalar();

            if (userCount == 0)
            {
                throw new InvalidOperationException("Користувача з таким ID не знайдено.");
            }

            SqlCommand command = new SqlCommand("DELETE FROM Users WHERE UserId = @id", connection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }
    }
}
