using TaskPanelLibrary.Entity;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelLibrary.Service.Interface;

public interface IPanelService
{
    public Panel CreatePanel(Panel panel);

    public List<Panel> GetAllPanelForTeam(int idTeam);
    
    public Panel UpdatePanel(Panel panelUpdated);

    public Panel DeletePanel(int panelId, User user);

    public Task AddTask(int panelId, Task task);
    
    public void AddTeam(int panelId, Team team);
    
    public void AddUser(int panelId, User user);

    public User RemoveUser(int panelId, User user);

    public Panel FindById(int panelId);
    
    public List<Panel> GetAllPanels();
}