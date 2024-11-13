using TaskPanelLibrary.Entity;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelLibrary.Service.Interface;

public interface ITrashService
{
    public Trash CreateTrash(User user);
    
    public void AddTaskToTrash(Task task, int trash);
    
    public void AddPanelToTrash(Panel panel, int trash);
    
    public Task RecoverTaskFromTrash(int taskId);
    
    public Panel RecoverPanelFromTrash(int panelId);
    
    public void DeleteTrash(int trashId);
    
    public Trash GetTrashById(int trashId);
}