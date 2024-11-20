using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Service.Interface;

namespace TaskPanelLibrary.Service;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    
    private User _currentUser;

    public AuthService(IUserService userService)
    {
        _userService = userService;
    }

    public User GetCurrentUser()
    {
        return _currentUser;
    }

    public bool IsLoggedIn()
    {
        return _currentUser != null;
    }

    public bool Login(string email, string password)
    {
        var users = _userService.GetAllUsers();
        _currentUser = users.FirstOrDefault(user => user.Email == email && user.Password == password);
        return _currentUser != null;
    }

    public void Logout()
    {
        _currentUser = null;
    }
}