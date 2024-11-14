using TaskPanelLibrary.Entity;

namespace TaskPanelLibrary.Service.Interface;

public interface IPanelService
{
    public Panel CreatePanel(Panel panel, int userId);

    public List<Panel> GetAllPanelForTeam(int teamId);
    
    public List<Panel> GetAllPanelForUser(int userId);
    
    public Panel UpdatePanel(Panel panelUpdated);

    public Panel DeletePanel(int panelId, User user);
    
    public Panel RestorePanel(int panelId, User user);

    public Panel GetPanelById(int panelId);
    
    public List<Panel> GetAllPanels();
    
}