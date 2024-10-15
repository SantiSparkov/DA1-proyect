using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Exception;
using TaskPanelLibrary.Exception.Panel;
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
        this._panelRepository = panelRepository;
        _userService = userService;
    }

    public Panel CreatePanel(Panel panel)
    {
        panel.Id = _panelRepository.Count() + 1;
        return _panelRepository.AddPanel(panel);
    }

    public List<Panel> GetAllPanelForTeam(int idTeam)
    {
        try
        {
            return _panelRepository.GetAll().FindAll(i => i.Team.Id == idTeam);
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
            return _panelRepository.GetAll().FindAll(i =>  i.Team.Users.Contains(user)).ToList();
        }
        catch(ArgumentException e)
        {
            return new List<Panel>();
        }
    }


    public Panel UpdatePanel(Panel panelUpdated)
    {
        Panel panelSaved = _panelRepository.FindById(panelUpdated.Id);
        panelSaved.Description = panelUpdated.Description ?? panelSaved.Description;
        panelSaved.Name = panelUpdated.Name ?? panelSaved.Name;
        return panelSaved;
    }

    public Panel DeletePanel(int panelId, User user)
    {
        if (!user.IsAdmin)
        {
            throw new PanelNotValidException($"User is not admin, userId: {user.Id}");
        }

        Panel panel = _panelRepository.Delete(panelId);
        user.Trash.AddPanel(panel);
        return panel;
    }

    public Panel FindById(int panelId)
    {
        return _panelRepository.FindById(panelId);
    }
    
    public List<Panel> GetAllPanels()
    {
        return _panelRepository.GetAll();
    }
    
}