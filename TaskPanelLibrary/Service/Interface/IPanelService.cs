using TaskPanelLibrary.Entity;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelLibrary.Service.Interface;

public interface IPanelService
{
    public Panel CreatePanel(Panel panel);

    public List<Panel> GetAllPanelForTeam(int idTeam);
    
    public List<Panel> GetAllPanelForUser(int idTeam);
    
    public Panel UpdatePanel(Panel panelUpdated);

    public Panel DeletePanel(int panelId, User user);
    
    public Panel RecoverPanel(int panelId, User user);

    public Panel GetPanelById(int panelId);
    
    public List<Panel> GetAllPanels();
}