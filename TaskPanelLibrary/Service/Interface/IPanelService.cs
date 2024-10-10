using TaskPanelLibrary.Entity;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelLibrary.Repository.Interface;

public interface IPanelService
{
    public Panel CreatePanel(User user);
    
    public Panel UpdatePanel(int panelId, Panel panel);

    public Panel DeletePanel(int panelId, User user);

    public Task AddTask(int panelId, Task task);

    public Task DeleteTask(Task task, User user);

    public void AddTeam(int panelId, Team team);
    
    public void AddUser(int panelId, User user);

    public User RemoveUser(int panelId, User user);

    public Panel FindById(int panelId);
}