using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Service.Interface;

namespace TaskPanelLibrary.Service;

public class AuthService
{
    private readonly IUserService _userServiceService;
    
    private User _currentUser;

    public AuthService(IUserService userServiceService)
    {
        _userServiceService = userServiceService;
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
        var users = _userServiceService.GetAllUsers();
        //_currentUser = users.FirstOrDefault(user => user.Email == email && user.Password == password);

        _currentUser = users.FirstOrDefault(user => "admin@admin.com" == email && user.Password == "admin");

        return _currentUser != null;
    }

    public void Logout()
    {
        _currentUser = null;
    }
}