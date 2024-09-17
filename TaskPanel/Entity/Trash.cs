namespace TaskPanel.Models.Entity;

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
        int count = quantityElemnt();
        if (count == 10)
        {
            throw new Exception("Papelera llena");
        }
        PanelList.Add(panel);
    }

    private int quantityElemnt()
    { 
        return PanelList.Count + TaskList.Count;
    }
    
}