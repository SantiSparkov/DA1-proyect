using TaskPanelLibrary.Entity;

namespace TaskPanelLibrary.Repository.Interface;

public interface IPanelRepository
{ 
    public Panel AddPanel(Panel panel);

    public Panel DeletePanel(int id);

    public Panel GetPanelById(int id);

    public Panel UpdatePanel(Panel panel);

    public List<Panel> GetAllPanels();

    public int Count();
}