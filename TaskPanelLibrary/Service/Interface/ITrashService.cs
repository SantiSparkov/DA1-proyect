using TaskPanelLibrary.Entity;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelLibrary.Service.Interface;

public interface ITrashService
{
    public Trash CreateTrash(User user);

    public void AddTaskToTrash(Task task, int trash);

    public void AddPanelToTrash(Panel panel, int trash);

    public void AddEpicToTrash(Epic epic, int userTrashId);

    public void RecoverEpicFromTrash(int epicId, int userTrashId);

    public Task RecoverTaskFromTrash(int taskId, int trashId);

    public Panel RecoverPanelFromTrash(int panelId, int trashId);

    public void DeleteTrash(int trashId);

    public bool IsFull(int trashId);

    public void RemoveEpicFromTrash(int epicId, int userTrashId);

    public void UpdateTrash(int trashId);

    public void RemoveTaskFromTrash(int taskId, int trashId);

    public void RemovePanelFromTrash(int panelId, int trashId);

    public Trash GetTrashById(int trashId);
}