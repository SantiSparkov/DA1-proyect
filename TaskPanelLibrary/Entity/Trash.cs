namespace TaskPanelLibrary.Entity;

public class Trash
{
    public List<Task> TaskList { get; }
    
    public List<Panel> PanelList { get; }

    public Trash()
    {
        TaskList = new List<Task>();
        PanelList = new List<Panel>();
    }

    public void addPanel(Panel panel)
    {
        CheckTrashSpace();
        PanelList.Add(panel);
    }
    
    public void addTask(Task task)
    {
        CheckTrashSpace();
        TaskList.Add(task);
    }

    
    private void CheckTrashSpace()
    {
        int count = quantityElemnt();
        if (count == 10)
        {
            throw new Exception("Papelera llena");
        }
        
    }

    private int quantityElemnt()
    { 
        return PanelList.Count + TaskList.Count;
    }
    
}