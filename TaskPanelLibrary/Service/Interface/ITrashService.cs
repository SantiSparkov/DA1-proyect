using TaskPanelLibrary.Entity;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelLibrary.Service.Interface;

public interface ITrashService
{
    public Trash CreateTrash(Trash trash, int userId);
    
    public void AddTaskToTrash(Task task, int trash);
    
    public void AddPanelToTrash(Panel panel, int trash);
    
    public Task RecoverTaskFromTrash(int taskId);
    
    public Panel RecoverPanelFromTrash(int panelId);
    
    public Trash GetTrashById(int trashId);
}