using TaskPanelLibrary.Entity;

namespace TaskPanelLibrary.Service.Interface;

public interface IUserService
{
    List<User> GetAllUsers();
    
    User GetUserById(int id);
    
    User AddUser(User user);
    
    User UpdateUser(User user);
    
    User DeleteUser(int id);
}
