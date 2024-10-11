using TaskPanelLibrary.Exception;

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

    public void AddPanel(Panel panel)
    {
        CheckTrashSpace();
        PanelList.Add(panel);
    }
    
    public void AddTask(Task task)
    {
        CheckTrashSpace();
        TaskList.Add(task);
    }
    
    private void CheckTrashSpace()
    {
        int count = QuantityElemnt();
        if (count >= 10)
        {
            throw new TaskPanelException("Papelera llena");
        }
    }

    private int QuantityElemnt()
    { 
        return PanelList.Count + TaskList.Count;
    }
    
}