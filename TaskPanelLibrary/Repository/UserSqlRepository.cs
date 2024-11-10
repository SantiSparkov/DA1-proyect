using TaskPanelLibrary.Config;

namespace TaskPanelLibrary.Repository.Interface;

using TaskPanelLibrary.Entity;

public class UserSqlRepository : IUserRepository
{

    private SqlContext _userDataBase;

    public UserSqlRepository(SqlContext sqlContext)
    {
        _userDataBase = sqlContext;

        if (!_userDataBase.Users.Any(u => u.Email == "admin@admin.com"))
        {
            var adminUser = new User
            {
                Name = "Admin",
                LastName = "User",
                Email = "admin@admin.com",
                Password = "Aa1@",
                IsAdmin = true,
                BirthDate = new DateTime(1990, 1, 1)
            };
            _userDataBase.Add(adminUser);
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