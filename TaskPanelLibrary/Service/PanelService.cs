using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Exception;
using TaskPanelLibrary.Repository.Interface;
using TaskPanelLibrary.Service.Interface;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelLibrary.Service;

public class PanelService : IPanelService
{ 
    private readonly IPanelRepository _panelRepository;
    
    public PanelService(IPanelRepository panelRepository)
    {
        this._panelRepository = panelRepository;
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
            throw new TaskPanelException($"User is not admin, userId: {user.Id}");
        }

        Panel panel = _panelRepository.Delete(panelId);
        user.Trash.AddPanel(panel);
        return panel;
    }


    public Task AddTask(int panelId, Task task)
    {
        IsValidTask(task);
        Panel panel = _panelRepository.FindById(panelId);
        panel.Tasks.Add(task);
        return task;
    }

    public void AddTeam(int panelId, Team team)
    {
        Panel panel = _panelRepository.FindById(panelId);
        foreach (var user in team.Users)
        {
            if (!ContainsInGroup(panel, user))
            {
                panel.Team.Users.Add(user);
            }
        }
    }

    public void AddUser(int panelId, User user)
    {
        Panel panel = _panelRepository.FindById(panelId);
        if (!ContainsInGroup(panel, user))
        {
            panel.Team.Users.Add(user);
        }
    }

    public User RemoveUser(int panelId, User user)
    {
        Panel panel = _panelRepository.FindById(panelId);

        if (ContainsInGroup(panel, user))
        {
            panel.Team.Users.Remove(user);
            return user;
        }
        else
        {
            throw new TaskPanelException($"User does not belong to the group, userId: {user.Id}");
        }
    }

    public Panel FindById(int panelId)
    {
        return _panelRepository.FindById(panelId);
    }

    private Boolean ContainsInGroup(Panel panel, User user)
    {
        return panel.Team.Users.Contains(user);
    }
    
    //Verificar si ya existe en team service
    private Team CreateTeamDefault(User user)
    {
        List<User> users = new List<User>();
        users.Add(user);
        Team team = new Team()
        {
            CreationDate = DateTime.Now,
            Id = 1,
            MaxAmountOfMembers = 20,
            Name = "Panel 1",
            TasksDescription = "Description",
            Users = users
        };
        return team;
    }
    
    public List<Panel> GetAllPanels()
    {
        return _panelRepository.GetAll();
    }
    
    private bool IsValidTask(Task task)
    {
        return task != null && !string.IsNullOrEmpty(task.Title) && !string.IsNullOrEmpty(task.Description);
    }
}