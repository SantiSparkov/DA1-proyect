using TaskPanelLibrary.Entity;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelLibrary.Service.Interface
{
    public interface IEpicService
    {
        public Epic CreateEpic(Epic epic, int panelId);
        
        public Epic GetEpicById(int id);
        
        public List<Epic> GetAllEpicsByPanelId(int panelId);
        
        public Epic UpdateEpic(Epic epic);
        
        public Epic DeleteEpic(int id);
        
        public void AddTaskToEpic(int epicId, int taskId);
        
        public void DeleteTaskFromEpic(int epicId, int taskId);
        
        public List<Task> GetTasksFromEpic(int epicId);
    }
}