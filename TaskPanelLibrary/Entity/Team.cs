namespace TaskPanelLibrary.Entity;

public class Team
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public DateTime CreationDate { get; set; }
    
    public string TasksDescription { get; set; }
    
    public int MaxAmountOfMembers { get; set; }
    
    public List<User> Users { get; set; }
    
    public List<Panel> Panels { get; set; }
    
    public User TeamLeader { get; set; }
    
    public Team()
    {
        Users = new List<User>();
        Panels = new List<Panel>();
    }
    
}