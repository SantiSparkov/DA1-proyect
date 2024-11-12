using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Exception.Panel;
using TaskPanelLibrary.Exception.Task;
using TaskPanelLibrary.Repository.Interface;
using TaskPanelLibrary.Service.Interface;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelLibrary.Service;

public class TrashService : ITrashService
{
    private readonly ITrashRepository _trashRepository;
    private readonly IUserService _userService;
    private const int MaxCapacity = 10;

    public TrashService(ITrashRepository trashRepository, IUserService userService)
    {
        _trashRepository = trashRepository;
        _userService = userService;
    }

    public Trash CreateTrash(Trash trash, int userId)
    {
        var user = _userService.GetUserById(userId);

        Trash newTrash = new Trash()
        {
            UserId = user.Id,
            Elements = 0
        };
        
        return _trashRepository.AddTrash(newTrash);
    }

    public void AddTaskToTrash(Task task, int trashId)
    {
        var trash = _trashRepository.GetTrashById(trashId);
        
        if (!IsFull(trashId))
        {
            trash.TaskList.Add(task);
            trash.Elements++;
        }
    }

    public void AddPanelToTrash(Panel panel, int trashId)
    {
        var trash = _trashRepository.GetTrashById(trashId);
        
        if (!IsFull(trashId))
        {
            trash.PanelList.Add(panel);
            trash.Elements++;
        }
    }

    public Task RecoverTaskFromTrash(int taskId)
    {
        var trash = _trashRepository.GetTrashById(taskId);
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
        var trash = _trashRepository.GetTrashById(panelId);
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
        var trash = _trashRepository.GetTrashById(trashId);
        
        return (trash.TaskList.Count + trash.PanelList.Count) >= MaxCapacity;
    }

    public Trash GetTrashById(int trashId)
    {
        return _trashRepository.GetTrashById(trashId);
    }
}


