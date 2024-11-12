using TaskPanelLibrary.Exception;

namespace TaskPanelLibrary.Entity;

public class Trash
{
    public int Id { get; set; }
    
    public List<Task> TaskList { get; }
    
    public List<Panel> PanelList { get; }
    
    public int UserId { get; set; }
    
    public int Elements { get; set; }

    public Trash()
    {
        TaskList = new List<Task>();
        PanelList = new List<Panel>();
    }
}