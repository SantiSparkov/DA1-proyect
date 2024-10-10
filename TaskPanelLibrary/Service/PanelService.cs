using System.Runtime.Serialization;
using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Exception;
using TaskPanelLibrary.Repository;
using TaskPanelLibrary.Repository.Interface;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelLibrary.Service;

public class PanelService : IPanelService
{
    private PanelRepository panelRepository;

    private UserService _userService;

    public PanelService(PanelRepository panelRepository)
    {
        this.panelRepository = panelRepository;
    }

    public Panel CreatePanel(User user)
    {
        int countPanel = panelRepository.Count();
        List<Task> tasks = new List<Task>();
        Panel panel = new Panel()
        {
            Id = countPanel++,
            Team = CreateTeamDefault(user),
            Description = "Description",
            Tasks = tasks,
            Name = "Name panel"
        };
        return panelRepository.AddPanel(panel);
    }

    public Panel UpdatePanel(int panelId, Panel panel)
    {
        Panel panelSaved = panelRepository.FindById(panelId);
        panelSaved.Description = panel.Description ?? panelSaved.Description;
        panelSaved.Name = panel.Name ?? panelSaved.Name;
        return panelSaved;
    }

    public Panel DeletePanel(int panelId, User user)
    {
        if (!user.IsAdmin)
        {
            throw new ApiException($"User is not admin, userId: {user.Id}");
        }

        Panel panel = panelRepository.Delete(panelId);
        user.Trash.AddPanel(panel);
        return panel;
    }


    public Task AddTask(int panelId, Task task)
    {
        Panel panel = panelRepository.FindById(panelId);
        panel.Tasks.Add(task);
        return task;
    }

    public Task DeleteTask(Task task, User user)
    {
        Panel panel = panelRepository.FindById(task.PanelId);
        panel.Tasks.Remove(task);
        user.Trash.AddTask(task);
        return task;
    }

    public void AddTeam(int panelId, Team team)
    {
        Panel panel = panelRepository.FindById(panelId);
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
        Panel panel = panelRepository.FindById(panelId);
        if (!ContainsInGroup(panel, user))
        {
            panel.Team.Users.Add(user);
        }
    }

    public User RemoveUser(int panelId, User user)
    {
        Panel panel = panelRepository.FindById(panelId);

        if (ContainsInGroup(panel, user))
        {
            panel.Team.Users.Remove(user);
            return user;
        }
        else
        {
            throw new ApiException($"User does not belong to the group, userId: {user.Id}");
        }
        
    }

    public Panel FindById(int panelId)
    {
        return panelRepository.FindById(panelId);
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
}