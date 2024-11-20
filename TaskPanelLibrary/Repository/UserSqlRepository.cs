using TaskPanelLibrary.Config;
using TaskPanelLibrary.Service.Interface;
using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Repository.Interface;

namespace TaskPanelLibrary.Repository;

public class UserSqlRepository : IUserRepository
{
    private readonly SqlContext _userDataBase;
    private readonly ITrashService _trashService;

    public UserSqlRepository(SqlContext sqlContext, ITrashService trashService)
    {
        _userDataBase = sqlContext;
        _trashService = trashService;

        if (!_userDataBase.Users.Any(u => u.Email == "admin@admin.com"))
        {
            var adminUser = new User
            {
                Name = "Admin",
                LastName = "User",
                Email = "admin@admin.com",
                Password = "Aa1@",
                IsAdmin = true,
                BirthDate = new DateTime(1990, 1, 1),
            };
            
            _userDataBase.Add(adminUser);
            _userDataBase.SaveChanges();
            
            _trashService.CreateTrash(adminUser);
            adminUser.TrashId = 1;
            _userDataBase.Update(adminUser);
            _userDataBase.SaveChanges();
            
        }
    }

    public User AddUser(User user)
    {
        _userDataBase.Users.Add(user);
        _userDataBase.SaveChanges();

        return user;
    }

    public User DeleteUser(int id)
    {
        User userDelete = _userDataBase.Users.Find(id);
        if (userDelete == null)
        {
            throw new System.Exception($"User with id: {id} does not exist");
        }

        _userDataBase.Remove(userDelete);
        _userDataBase.SaveChanges();
        return userDelete;
    }

    public User GetUserById(int id)
    {
        User user = _userDataBase.Users.Find(id);
        if (user == null)
        {
            throw new System.Exception($"User with id: {id} does not exist");
        }

        return user;
    }

    public List<User> GetAllUsers()
    {
        return _userDataBase.Users.ToList();
    }

    public User UpdateUser(User user)
    {
        _userDataBase.Users.Update(user);
        _userDataBase.SaveChanges();
        return user;
    }
}