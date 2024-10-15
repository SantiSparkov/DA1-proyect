using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Exception.User;
using TaskPanelLibrary.Repository;
using TaskPanelLibrary.Repository.Interface;
using TaskPanelLibrary.Service.Interface;

namespace TaskPanelLibrary.Service;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    private readonly PasswordGeneratorService _passwordGenerator;

    private const int PASSWORD_LENGTH = 8;

    public UserService(IUserRepository userRepository, PasswordGeneratorService passwordGenerator)
    {
        _userRepository = userRepository;
        _passwordGenerator = passwordGenerator;
    }

    public List<User> GetAllUsers()
    {
        return _userRepository.GetAllUsers();
    }

    public User GetUserById(int id)
    {
        var foundUser = _userRepository.GetUserById(id);

        if (foundUser == null)
        {
            throw new UserNotValidException(id);
        }

        return foundUser;
    }

    public User AddUser(User user)
    {
        var users = GetAllUsers();
        bool exists = users.Exists(actualUser => actualUser.Email == user.Email);
        if (exists)
        {
            throw new UserNotValidException("User already exists");
        }
        return _userRepository.AddUser(user);
    }

    public User UpdateUser(User user)
    {
        var existingUser = _userRepository.GetUserById(user.Id);
        if (existingUser == null)
        {
            throw new UserNotValidException(user.Id);
        }

        return _userRepository.UpdateUser(user);
    }

    public User DeleteUser(int id)
    {
        var existingUser = _userRepository.GetUserById(id);
        if (existingUser == null)
        {
            throw new UserNotValidException(id);
        }

        return _userRepository.DeleteUser(id);
    }
}