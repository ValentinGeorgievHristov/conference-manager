namespace ConferenceManager.API.Services.Users
{
    using ConferenceManager.API.DTOs.Users;
    using ConferenceManager.API.Models;

    public interface IUserService
    {
        List<UserDto> GetAllUsers();

        void CreateUser(CreateUserDto dto);

        string Login(LoginDto dto);

        bool DeleteUser(int id);

        bool UpdateUser(int id, CreateUserDto dto);

    }
}

