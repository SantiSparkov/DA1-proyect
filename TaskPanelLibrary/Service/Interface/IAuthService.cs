using TaskPanelLibrary.Entity;

namespace TaskPanelLibrary.Service.Interface;

public interface IAuthService
{
    public User GetCurrentUser();

    public bool IsLoggedIn();

    public bool Login(string email, string password);

    public void Logout();
}