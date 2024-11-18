public class UserDto
{
    public int UserId { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public DateTime? CreatedAt { get; set; }
    public int RoleID { get; set; }
    public string? RoleName { get; set; }
}
