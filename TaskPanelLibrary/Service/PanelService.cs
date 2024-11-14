using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Exception;
using TaskPanelLibrary.Exception.Panel;
using TaskPanelLibrary.Repository;
using TaskPanelLibrary.Repository.Interface;
using TaskPanelLibrary.Service.Interface;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelLibrary.Service;

public class PanelService : IPanelService
{
    private readonly IPanelRepository _panelRepository;

    private IUserService _userService;

    private ITrashService _trashService;

    private ITeamService _teamService;

    public PanelService(IPanelRepository panelRepository, IUserService userService, ITrashService trashService, ITeamService teamService)
    {
        _panelRepository = panelRepository;
        _userService = userService;
        _trashService = trashService;
        _teamService = teamService;
    }

    public Panel CreatePanel(Panel panel, int userId)
    {
        var user = _userService.GetUserById(userId);
        panel.CreatorId = user.Id;
        
        if (!IsValidPanel(panel))
            throw new PanelNotValidException("Panel is not valid");

        var panelSaved = _panelRepository.AddPanel(panel);
        return panelSaved;
    }

    public List<Panel> GetAllPanelForTeam(int idTeam)
    {
        return _panelRepository.GetAllPanels().FindAll(i => i.Team.Id == idTeam);
    }

    public List<Panel> GetAllPanelForUser(int userId)
    {
        List<Team> teams = _teamService.TeamsForUser(userId) ?? new List<Team>();
        return _panelRepository.GetAllPanels().FindAll(i => teams.Contains(i.Team)).ToList();
    }

    public Panel UpdatePanel(Panel panelUpdated)
    {
        Panel panelSaved = _panelRepository.GetPanelById(panelUpdated.Id);
        _panelRepository.UpdatePanel(panelUpdated);
        return panelSaved;
    }

    public Panel DeletePanel(int panelId, User user)
    {
        var panel = _panelRepository.GetPanelById(panelId);

        if (!user.IsAdmin)
        {
            throw new PanelNotValidException($"User is not admin, userId: {user.Id}");
        }

        if (panel.IsDeleted)
        {
            _trashService.RemovePanelFromTrash(panelId, user.TrashId);
            _panelRepository.DeletePanel(panelId);
        }
        else
        {
            panel.IsDeleted = true;
            _trashService.AddPanelToTrash(panel, user.TrashId);
            _panelRepository.UpdatePanel(panel);
        }

        return panel;
    }

    
    public Panel RestorePanel(int panelId, User user)
    {
        var panel = _panelRepository.GetPanelById(panelId);

        if (!user.IsAdmin)
        {
            throw new PanelNotValidException($"User is not admin, userId: {user.Id}");
        }

        if (_trashService.GetTrashById(user.TrashId).PanelList.Contains(panel))
        {
            _trashService.RecoverPanelFromTrash(panelId, user.TrashId);
            panel.IsDeleted = false;
            _panelRepository.UpdatePanel(panel);
        }

        return panel;
    }
    
    public Panel GetPanelById(int panelId)
    {
        return _panelRepository.GetPanelById(panelId);
    }

    public List<Panel> GetAllPanels()
    {
        return _panelRepository.GetAllPanels();
    }

    private bool IsValidPanel(Panel panel)
    {
        if (panel == null)
            throw new PanelNotValidException("Panel is null");

        if (string.IsNullOrEmpty(panel.Name))
            throw new PanelNotValidException("Panel name is null or empty");

        if (string.IsNullOrEmpty(panel.Description))
            throw new PanelNotValidException("Panel description is null or empty");

        if (panel.Team == null)
            throw new PanelNotValidException("Panel team is null");

        return true;
    }
}