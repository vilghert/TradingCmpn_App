public interface IUserDal
{
    List<UserDto> GetAll();
    UserDto GetById(int id);
    void Insert(UserDto user);
    void Update(UserDto user);
    void Delete(int id);
}