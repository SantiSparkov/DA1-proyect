using TaskPanelLibrary.Entity;

namespace TaskPanelLibrary.Repository.Interface;

public interface IUserRepository
{
    User AddUser(User user);
    
    User DeleteUser(int id);
    
    User GetUserById(int id);
    
    List<User> GetAllUsers();
    
    User UpdateUser(User user);
}