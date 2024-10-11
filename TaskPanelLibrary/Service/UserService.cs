using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Exception.User;
using TaskPanelLibrary.Repository;
using TaskPanelLibrary.Repository.Interface;
using TaskPanelLibrary.Service.Interface;

namespace TaskPanelLibrary.Service;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository = new UserRepository();

    public List<User> GetAllUsers()
    {
        return _userRepository.GetAllUsers();
    }

    public User GetUserById(int id)
    {
        var foundUser = _userRepository.GetUserById(id);

        if (foundUser == null)
        {
            throw new UserNotFoundException(id);
        }

        return foundUser;
    }

    public User AddUser(User user)
    {
        var users = GetAllUsers();
        bool exists = users.Exists(actualUser => actualUser.Email == user.Email);
        if (exists)
        {
            throw new UserAlreadyExistsException(user.Email);
        }
        
        return _userRepository.AddUser(user);
    }

    public User UpdateUser(User user)
    {
        var existingUser = _userRepository.GetUserById(user.Id);
        if (existingUser == null)
        {
            throw new UserNotFoundException(user.Id);
        }

        return _userRepository.UpdateUser(user);
    }

    public User DeleteUser(int id)
    {
        var existingUser = _userRepository.GetUserById(id);
        if (existingUser == null)
        {
            throw new UserNotFoundException(id);
        }

        return _userRepository.DeleteUser(id);
    }
}

