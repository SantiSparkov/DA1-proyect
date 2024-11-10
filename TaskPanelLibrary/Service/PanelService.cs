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
    
    public PanelService(IPanelRepository panelRepository, IUserService userService)
    {
        _panelRepository = panelRepository;
        _userService = userService;
    }

    public Panel CreatePanel(Panel panel)
    {
        if (!IsValidPanel(panel))
            throw new PanelNotValidException("Panel is not valid");
        
        var panelSaved = _panelRepository.AddPanel(panel);
        return panelSaved;
    }

    public List<Panel> GetAllPanelForTeam(int idTeam)
    {
        try
        {
            return _panelRepository.GetAllPanels().FindAll(i => i.Team.Id == idTeam);
        }
        catch(ArgumentException e)
        {
            return new List<Panel>();
        }

    }

    public List<Panel> GetAllPanelForUser(int userId)
    {
        try
        {
            User user = _userService.GetUserById(userId);
            return _panelRepository.GetAllPanels().FindAll(i =>  i.Team.Users.Contains(user)).ToList();
        }
        catch(ArgumentException e)
        {
            return new List<Panel>();
        }
    }


    public Panel UpdatePanel(Panel panelUpdated)
    {
        Panel panelSaved = _panelRepository.GetPanelById(panelUpdated.Id);
        _panelRepository.UpdatePanel(panelUpdated);
        return panelSaved;
    }

    public Panel DeletePanel(int panelId, User user)
    {
        if (!user.IsAdmin)
        {
            throw new PanelNotValidException($"User is not admin, userId: {user.Id}");
        }

        Panel panel = _panelRepository.DeletePanel(panelId);
        user.Trash.AddPanel(panel);
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