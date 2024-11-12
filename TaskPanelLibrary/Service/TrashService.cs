using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Exception.Panel;
using TaskPanelLibrary.Exception.Task;
using TaskPanelLibrary.Repository.Interface;
using TaskPanelLibrary.Service.Interface;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelLibrary.Service;

public class TrashService : ITrashService
{
    private readonly ITrashRepository _trashSqlRepository;
    private const int MaxCapacity = 10;

    public TrashService(ITrashRepository trashSqlRepository)
    {
        _trashSqlRepository = trashSqlRepository;
    }

    public Trash CreateTrash(int userId)
    {
        Trash newTrash = new Trash()
        {
            UserId = userId,
            Elements = 0
        };
        
        return _trashSqlRepository.AddTrash(newTrash);
    }

    public void AddTaskToTrash(Task task, int trashId)
    {
        var trash = _trashSqlRepository.GetTrashById(trashId);
        
        if (!IsFull(trashId))
        {
            trash.TaskList.Add(task);
            trash.Elements++;
        }
    }

    public void AddPanelToTrash(Panel panel, int trashId)
    {
        var trash = _trashSqlRepository.GetTrashById(trashId);
        
        if (!IsFull(trashId))
        {
            trash.PanelList.Add(panel);
            trash.Elements++;
        }
    }

    public Task RecoverTaskFromTrash(int taskId)
    {
        var trash = _trashSqlRepository.GetTrashById(taskId);
        var task = trash.TaskList.Find(t => t.Id == taskId);
        
        if (task == null)
        {
            throw new TaskNotValidException("Task not found in trash");
        }
        
        trash.TaskList.Remove(task);
        trash.Elements--;
        
        return task;
    }

    public Panel RecoverPanelFromTrash(int panelId)
    {
        var trash = _trashSqlRepository.GetTrashById(panelId);
        var panel = trash.PanelList.Find(p => p.Id == panelId);
        
        if (panel == null)
        {
            throw new PanelNotValidException("Panel not found in trash");
        }
        
        trash.PanelList.Remove(panel);
        trash.Elements--;
        
        return panel;
    }

    private bool IsFull(int trashId)
    {
        var trash = _trashSqlRepository.GetTrashById(trashId);
        
        return (trash.TaskList.Count + trash.PanelList.Count) >= MaxCapacity;
    }

    public Trash GetTrashById(int trashId)
    {
        return _trashSqlRepository.GetTrashById(trashId);
    }
}


