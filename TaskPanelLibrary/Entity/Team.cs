namespace TaskPanelLibrary.Entity;

public class Team
{
    public string Name { get; set; }
    
    public DateTime CreationDate { get; set; }
    
    public string TasksDescription { get; set; }
    
    public int MaxAmountOfMembers { get; set; }
    
    public List<User> Users { get; set; }
    
}