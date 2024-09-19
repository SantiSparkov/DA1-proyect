namespace TaskPanel.Models.Entity;

public class Panel
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public Team Team { get; set; }
    
    public string Description { get; set; }
    
    public List<Task> Tasks { get; set; }
    
}
