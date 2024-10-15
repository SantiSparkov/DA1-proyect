using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Exception.User;
using TaskPanelLibrary.Repository.Interface;

namespace TaskPanelLibrary.Repository;

public class UserRepository : IUserRepository
{
    private readonly List<User> _users = new();
    
    public UserRepository()
    {
        var adminUser = new User
        {
            Id = 1,
            Name = "Admin",
            LastName = "User",
            Email = "admin@admin.com",
            Password = "admin",
            IsAdmin = true,
            BirthDate = new DateTime(1990, 1, 1)
        };

        _users.Add(adminUser);
    }

    public User AddUser(User user)
    {
        user.Id = _users.Count > 0 ? _users.Max(u => u.Id) + 1 : 1;
        _users.Add(user);
        return user;
    }

    public User DeleteUser(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user != null)
        {
            _users.Remove(user);
        }
        return user;
    }

    public User GetUserById(int id)
    {
        return _users.FirstOrDefault(u => u.Id == id);
    }

    public List<User> GetAllUsers()
    {
        return _users;
    }

    public User UpdateUser(User user)
    {
        var existingUser = _users.FirstOrDefault(u => u.Id == user.Id);
        if (existingUser != null)
        {
            existingUser.Name = user.Name ?? existingUser.Name;
            existingUser.LastName = user.LastName ?? existingUser.LastName;
            existingUser.BirthDate = user.BirthDate != default(DateTime) ? user.BirthDate : existingUser.BirthDate;
            existingUser.IsAdmin = user.IsAdmin;
            return existingUser;
        }
        else
        {
            throw new UserNotValidException(user.Email);
        }
    }
}