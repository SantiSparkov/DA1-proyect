using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Exception.User;
using TaskPanelLibrary.Repository;
using TaskPanelLibrary.Repository.Interface;
using TaskPanelLibrary.Service.Interface;

namespace TaskPanelLibrary.Service;

public class UserService : IUserService
{
    private readonly UserSqlRepository _userSqlRepository;

    private readonly PasswordGeneratorService _passwordGenerator;

    private const int PASSWORD_LENGTH = 8;

    public UserService(UserSqlRepository userSqlRepository, PasswordGeneratorService passwordGenerator)
    {
        _userSqlRepository = userSqlRepository;
        _passwordGenerator = passwordGenerator;
    }

    public List<User> GetAllUsers()
    {
        return _userSqlRepository.GetAllUsers();
    }

    public User GetUserById(int id)
    {
        var foundUser = _userSqlRepository.GetUserById(id);

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
        if (exists || !IsUserValid(user))
        {
            throw new UserNotValidException("User already exists");
        }
        return _userSqlRepository.AddUser(user);
    }

    public User UpdateUser(User user)
    {
        var existingUser = _userSqlRepository.GetUserById(user.Id);
        if (existingUser == null)
        {
            throw new UserNotValidException(user.Id);
        }

        return _userSqlRepository.UpdateUser(user);
    }

    public User DeleteUser(int id)
    {
        var existingUser = _userSqlRepository.GetUserById(id);
        if (existingUser == null)
        {
            throw new UserNotValidException(id);
        }

        return _userSqlRepository.DeleteUser(id);
    }
    
    private bool IsUserValid(User? user)
    {
        if (user == null)
            throw new UserNotValidException("User is null");
        if (string.IsNullOrEmpty(user.Email))
            throw new UserNotValidException("Email is null or empty");
        if (string.IsNullOrEmpty(user.Name))
            throw new UserNotValidException("Name is null or empty");
        if (string.IsNullOrEmpty(user.LastName))
            throw new UserNotValidException("LastName is null or empty");
        if (user.BirthDate > DateTime.Now)
            throw new UserNotValidException("BirthDate can't be in the future");
        if (user.BirthDate < new DateTime(1900, 1, 1))
            throw new UserNotValidException("BirthDate can't be before 1900");
        
        return true;
    }
}