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
        
        private readonly ITrashService _trashService;
        
        public EpicService(IEpicRepository epicRepository, IPanelService panelService, ITaskService taskService, ITrashService trashService)
        {
            _epicRepository = epicRepository;
            _panelService = panelService;
            _taskService = taskService;
            _trashService = trashService;
        }

        public Epic CreateEpic(Epic epic, int panelId)
        {
            IsValidEpic(epic);
            
            var panel = _panelService.GetPanelById(panelId);
            epic.PanelId = panel.Id;

            var epicSaved = _epicRepository.AddEpic(epic);
            return epicSaved;
        }

        public Epic GetEpicById(int id)
        {
            return _epicRepository.GetEpicById(id);
        }

        public List<Epic> GetAllEpicsByPanelId(int panelId)
        {
            List<Task> tasks = _taskService.GetAllTasks();
            return _epicRepository.GetAllEpics().Where(e => e.PanelId == panelId).ToList();
        }

        public Epic UpdateEpic(Epic epicUpdated)
        {
            Epic epicSaved = _epicRepository.GetEpicById(epicUpdated.Id);
            _epicRepository.UpdateEpic(epicUpdated);
            return epicSaved;
        }

        public Epic DeleteEpic(int id, User user)
        {
            var epic = _epicRepository.GetEpicById(id);
            
            if (epic.IsDeleted)
            {
                _trashService.RemoveEpicFromTrash(epic.Id, user.TrashId);
                _epicRepository.DeleteEpic(epic.Id);
            }
            else 
            {
                epic.IsDeleted = true;
                _trashService.AddEpicToTrash(epic, user.TrashId);
                _epicRepository.UpdateEpic(epic);
            }
            
            return epic;
        }
        
        public Epic RestoreEpic(int epicId, User user)
        {
            var epic = _epicRepository.GetEpicById(epicId);

            if (_trashService.GetTrashById(user.TrashId).EpicList.Contains(epic))
            {
                _trashService.RecoverEpicFromTrash(epicId, user.TrashId);
                epic.IsDeleted = false;
                _epicRepository.UpdateEpic(epic);
            }

            return epic;
        }

        private void IsValidEpic(Epic? epic)
        {
            if (epic == null)
                throw new EpicNotValidException("Epic is null");
            if (string.IsNullOrEmpty(epic.Title))
                throw new EpicNotValidException("Epic title is null or empty");
            if (string.IsNullOrEmpty(epic.Description))
                throw new EpicNotValidException("Epic description is null or empty");
        }
    }
}