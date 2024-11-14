using TaskPanelLibrary.Service.Interface;
using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Exception;
using TaskPanelLibrary.Repository.Interface;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelLibrary.Service
{
    public class EpicService : IEpicService
    {
        private readonly IEpicRepository _epicRepository;
        private readonly IPanelService _panelService;
        private readonly ITaskService _taskService;

        public EpicService(IEpicRepository epicRepository, IPanelService panelService, ITaskService taskService)
        {
            _epicRepository = epicRepository;
            _panelService = panelService;
            _taskService = taskService;
        }

        public Epic CreateEpic(Epic epic, int panelId)
        {
            var panel = _panelService.GetPanelById(panelId);
            epic.PanelId = panel.Id;

            if (!IsValidEpic(epic))
            {
                throw new EpicNotValidException("Epic is not valid");
            }

            var epicSaved = _epicRepository.AddEpic(epic);
            return epicSaved;
        }

        public Epic GetEpicById(int id)
        {
            return _epicRepository.GetEpicById(id);
        }

        public List<Epic> GetAllEpicsByPanelId(int panelId)
        {
            return _epicRepository.GetAllEpics().Where(e => e.PanelId == panelId).ToList();
        }

        public Epic UpdateEpic(Epic epicUpdated)
        {
            Epic epicSaved = _epicRepository.GetEpicById(epicUpdated.Id);
            _epicRepository.UpdateEpic(epicUpdated);
            return epicSaved;
        }

        public Epic DeleteEpic(int id)
        {
            var epic = _epicRepository.GetEpicById(id);

            if (epic.Tasks.Count > 0)
            {
                throw new EpicNotValidException("Epic has tasks");
            }

            return _epicRepository.DeleteEpic(id);
        }
         
        public void AddTaskToEpic(int epicId, int taskId)
        {
            var epic = _epicRepository.GetEpicById(epicId);
            var task = _taskService.GetTaskById(taskId);

            if (_epicRepository.GetAllEpics().Any(e => e.Tasks.Contains(task)))
            {
                throw new EpicNotValidException("Task already in epic");
            }

            epic.Tasks.Add(task);
            _epicRepository.UpdateEpic(epic);
        }
        
        public void DeleteTaskFromEpic(int epicId, int taskId)
        {
            var epic = _epicRepository.GetEpicById(epicId);
            var task = _taskService.GetTaskById(taskId);

            if (!epic.Tasks.Contains(task))
            {
                throw new EpicNotValidException("Task not in epic");
            }

            epic.Tasks.Remove(task);
            _epicRepository.UpdateEpic(epic);
        }
        
        public List<Task> GetTasksFromEpic(int epicId)
        {
            var epic = _epicRepository.GetEpicById(epicId);
            return epic.Tasks;
        }
        
        private bool IsValidEpic(Epic? epic)
        {
            if (epic == null)
                throw new EpicNotValidException("Epic is null");
            if (string.IsNullOrEmpty(epic.Title))
                throw new EpicNotValidException("Epic title is null or empty");
            if (string.IsNullOrEmpty(epic.Description))
                throw new EpicNotValidException("Epic description is null or empty");
            if (epic.DueDateTime == null)
                throw new EpicNotValidException("Epic due date is null");
            if (epic.Priority == null)
                throw new EpicNotValidException("Epic priority is null");
            
            return true;
        }
    }
}