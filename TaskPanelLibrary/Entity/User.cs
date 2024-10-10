namespace TaskPanelLibrary.Entity;

public class User
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string Email { get; set; }
    
    public string Password { get; set; }
    
    public string LastName { get; set; }
    
    public DateTime BirthDate { get; set; }
    
    public bool IsAdmin { get; set; }
    
    public Trash Trash { get; set; }


    public User()
    {
        Trash = new Trash();
    }

    public void DeleteTask(Task task)
    {
        Trash.AddTask(task);
    }

    public Trash getTrash()
    {
        return Trash;
    }
}