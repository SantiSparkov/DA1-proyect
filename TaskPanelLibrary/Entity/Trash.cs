using TaskPanelLibrary.Exception;

namespace TaskPanelLibrary.Entity;

public class Trash
{
    public int Id { get; set; }
    
    public List<Task> TaskList { get; set; }
    
    public List<Panel> PanelList { get; set; }
    
    public List<Epic> EpicList { get; set; }
    
    public int UserId { get; set; }
    
    public int Elements { get; set; }

    public Trash()
    {
        TaskList = new List<Task>();
        PanelList = new List<Panel>();
        EpicList = new List<Epic>();
    }
}