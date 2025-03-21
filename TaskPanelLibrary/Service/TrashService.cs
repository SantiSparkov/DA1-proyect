using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Exception;
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

    public Trash CreateTrash(User user)
    {
        Trash newTrash = new Trash()
        {
            UserId = user.Id,
            Elements = 0,
            TaskList = new List<Task>(),
            PanelList = new List<Panel>()
        };
        
        return _trashSqlRepository.AddTrash(newTrash);
    }

    public void AddTaskToTrash(Task task, int trashId)
    {
        var trash = _trashSqlRepository.GetTrashById(trashId);
        
        if (!IsFull(trashId))
        {
            trash.TaskList.Add(task);
            trash.Elements = Count(trashId);
        }
    }

    public void AddPanelToTrash(Panel panel, int trashId)
    {
        var trash = _trashSqlRepository.GetTrashById(trashId);
        
        if (!IsFull(trashId))
        {
            trash.PanelList.Add(panel);
            trash.Elements = Count(trashId);
        }
    }
    
    public void AddEpicToTrash(Epic epic, int userTrashId)
    {
        var trash = _trashSqlRepository.GetTrashById(userTrashId);
        
        if (!IsFull(userTrashId))
        {
            trash.EpicList.Add(epic);
            trash.Elements = Count(userTrashId);
        }
    }

    public void RecoverEpicFromTrash(int epicId, int userTrashId)
    {
        var trash = _trashSqlRepository.GetTrashById(userTrashId);
        var epic = trash.EpicList.Find(e => e.Id == epicId);
        
        if (epic == null)
        {
            throw new EpicNotValidException("Epic not found in trash");
        }
        
        trash.EpicList.Remove(epic);
        trash.Elements = Count(userTrashId);
    }

    public void RemoveTaskFromTrash(int taskId, int trashId)
    {
        var trash = _trashSqlRepository.GetTrashById(trashId);
        var task = trash.TaskList.FirstOrDefault(t => t.Id == taskId);
        if (task != null)
        {
            trash.TaskList.Remove(task);
            trash.Elements = Count(trashId);
            _trashSqlRepository.UpdateTrash(trash);
        }
    }

    public Task RecoverTaskFromTrash(int taskId, int trashId)
    {
        var trash = _trashSqlRepository.GetTrashById(trashId);
        var task = trash.TaskList.Find(t => t.Id == taskId);
        
        if (task == null)
        {
            throw new TaskNotValidException("Task not found in trash");
        }
        
        trash.TaskList.Remove(task);
        trash.Elements = Count(trashId);
        
        return task;
    }
        
    public void RemoveEpicFromTrash(int epicId, int userTrashId)
    {
        var trash = _trashSqlRepository.GetTrashById(userTrashId);
        var epic = trash.EpicList.FirstOrDefault(e => e.Id == epicId);
        if (epic != null)
        {
            trash.EpicList.Remove(epic);
            trash.Elements = Count(userTrashId);
            _trashSqlRepository.UpdateTrash(trash);
        }
    }

    public void RemovePanelFromTrash(int panelId, int trashId)
    {
        var trash = _trashSqlRepository.GetTrashById(trashId);
        var panel = trash.PanelList.FirstOrDefault(p => p.Id == panelId);
        if (panel != null)
        {
            trash.PanelList.Remove(panel);
            trash.Elements = Count(trashId);
            _trashSqlRepository.UpdateTrash(trash);
        }
    }
    
    public Panel RecoverPanelFromTrash(int panelId, int trashId)
    {
        var trash = _trashSqlRepository.GetTrashById(trashId);
        var panel = trash.PanelList.Find(p => p.Id == panelId);
        
        if (panel == null)
        {
            throw new PanelNotValidException("Panel not found in trash");
        }
        
        trash.PanelList.Remove(panel);
        trash.Elements = Count(trashId);
        
        return panel;
    }

    public void DeleteTrash(int trashId)
    {
        _trashSqlRepository.DeleteTrashForId(trashId);
    }

    public bool IsFull(int trashId)
    {
        var trash = _trashSqlRepository.GetTrashById(trashId);
        return (trash.TaskList.Count + trash.PanelList.Count + trash.EpicList.Count) >= MaxCapacity;
    }

    private int Count(int trashId)
    {
        var trash = _trashSqlRepository.GetTrashById(trashId);
        return (trash.TaskList.Count + trash.PanelList.Count);
    }
    
    public void UpdateTrash(int trashId)
    {
        var trash = _trashSqlRepository.GetTrashById(trashId);
        trash.Elements = Count(trashId);
        _trashSqlRepository.UpdateTrash(trash);
    }

    public Trash GetTrashById(int trashId)
    {
        return _trashSqlRepository.GetTrashById(trashId);
    }
}


