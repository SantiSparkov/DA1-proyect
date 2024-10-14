using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Exception.Panel;
using TaskPanelLibrary.Exception.Task;
using TaskPanelLibrary.Exception.Team;
using TaskPanelLibrary.Exception.User;
using DateTime = System.DateTime;
using TaskPanelLibrary.Repository.Interface;
using TaskPanelLibrary.Service.Interface;
using Team = TaskPanelLibrary.Entity.Team;

namespace TaskPanelLibrary.Service;

public class TeamService : ITeamService
{
    private readonly ITeamRepository _teamRepository;

    private readonly IUserService _userService;
    
    private readonly IPanelService _panelService;
    
    public TeamService(ITeamRepository teamRepository, IUserService userService, IPanelService panelService)
    {
        _teamRepository = teamRepository;
        _userService = userService;
        _panelService = panelService;
    }

    public Team CreateTeam(Team team, int userId)
    {
        var user = _userService.GetUserById(userId);
        
        if (!IsValidTeam(team, user))
        {
            throw new UserNotValidException("User is not admin");
        }
        
        Team newTeam = new Team
        {
            Name = team.Name,
            CreationDate = DateTime.Now,
            TasksDescription = team.TasksDescription,
            MaxAmountOfMembers = team.MaxAmountOfMembers,
            TeamLeader = user,
            Users = team.Users
        };

        newTeam.Users.Add(user);

        return _teamRepository.AddTeam(newTeam);
    }

    public Team DeleteTeam(Team team, int userId)
    {
        var user = _userService.GetUserById(userId);
        if (!CanDeleteTeam(team, user))
        {
            throw new UserNotValidException("User is not admin");
        }

        return _teamRepository.DeleteTeam(team.Id);
    }

    public Team UpdateTeam(Team team, int userId)
    {
        var user = _userService.GetUserById(userId);
        if (!CanUpdateTeam(user, team))
        {
            throw new UserNotValidException("User is not admin");
        }

        return _teamRepository.UpdateTeam(team);
    }

    public Team GetTeamById(int id)
    {
        var team = _teamRepository.GetTeamById(id);
        return team;
    }

    public List<Team> GetAllTeams()
    {
        var teams = _teamRepository.GetAllTeams();
        return teams;
    }

    public void AddUserToTeam(int userId, Team team)
    {
        var user = _userService.GetUserById(userId);

        if (!CanAddUserToTeam(user, team))
        {
            throw new UserNotValidException("User is not admin");
        }

        team.Users.Add(user);
        _teamRepository.UpdateTeam(team);
    }

    public void RemoveUserFromTeam(int userId, Team team)
    {
        var user = _userService.GetUserById(userId);

        if (!CanRemoveUserFromTeam(user, team))
        {
            throw new UserNotValidException("User is not admin");
        }

        team.Users.Remove(user);
        _teamRepository.UpdateTeam(team);
    }

    public void AddPanelToTeam(int panelId, Team team)
    {
        var panel = _panelService.FindById(panelId);

        if (!CanAddPanelToTeam(panel, team))
        {
            throw new UserNotValidException("User is not admin");
        }

        team.Panels.Add(panel);
        _teamRepository.UpdateTeam(team);
    }

    public void RemovePanelFromTeam(int panelId, Team team)
    {
        var panel = _panelService.FindById(panelId);

        if (!CanRemovePanelFromTeam(panel, team))
        {
            throw new UserNotValidException("User is not admin");
        }

        team.Panels.Remove(panel);
        _teamRepository.UpdateTeam(team);
    }
    
    public List<Team> TeamsForUser(int userId)
    {
        List<Team> result = new List<Team>();
        List<Team> teams = _teamRepository.GetAllTeams();
        foreach (Team team in teams)
        {
            List<User> users = team.Users;
            users.Where(i => i.Id == userId).ToList();
            if (users.Count > 0)
            {
                result.Add(team);
            }
        }
        return result;
    }

    private bool CanUpdateTeam(User updater, Team updatedTeam)
    {
        Team existingTeam = _teamRepository.GetTeamById(updatedTeam.Id);

        if (!updater.IsAdmin || updater.Id != existingTeam.TeamLeader.Id)
            throw new UserNotValidException("User is not admin or team leader.");

        if (string.IsNullOrEmpty(updatedTeam.Name) && updatedTeam.Name != existingTeam.Name)
            throw new ArgumentException("Invalid or duplicate team name.");

        if (updatedTeam.MaxAmountOfMembers <= 0)
            throw new ArgumentException("Invalid maximum number of users. Must be greater than zero.");

        if (updatedTeam.MaxAmountOfMembers < existingTeam.Users.Count)
            throw new InvalidOperationException(
                "Cannot set the maximum number of users lower than the current number of users.");

        if (string.IsNullOrEmpty(updatedTeam.TasksDescription))
            throw new ArgumentException("Tasks description cannot be null or empty.");

        return true;
    }

    private bool CanDeleteTeam(Team team, User user)
    {
        if (team.Panels.Count > 0)
            throw new InvalidOperationException("Team cannot be deleted because it has associated panels.");

        if (!user.IsAdmin)
            throw new UnauthorizedAccessException("Only an administrator can delete this team.");

        return true;
    }

    private bool IsValidTeam(Team team, User user)
    {
        if (!user.IsAdmin)
            throw new UserNotValidException("User is not admin");

        if (string.IsNullOrEmpty(team.Name))
            throw new TeamNotValidException("Team name is null");

        if (team.MaxAmountOfMembers <= 0)
            throw new TeamNotValidException("Max amount of members is zero or negative");

        if (!IsTeamNameUnique(team.Name))
            throw new TeamNotValidException("Team name is not unique");

        if (string.IsNullOrEmpty(team.TasksDescription))
            throw new TaskNotValidException("Tasks description is null");

        return true;
    }

    private bool CanAddUserToTeam(User user, Team team)
    {
        if (IsTeamFull(team))
            throw new TeamNotValidException("Team is full");

        if (team.Users.Contains(user))
            throw new UserNotFoundException(user.Email);

        return true;
    }

    private bool CanRemoveUserFromTeam(User user, Team team)
    {
        if (!team.Users.Contains(user))
            throw new UserNotFoundException(user.Email);

        if (team.TeamLeader == user)
            throw new UserNotValidException("User is team leader");

        if (team.Users.Count == 1)
            throw new TeamNotValidException("Team cannot be empty");
        return true;
    }

    private bool CanAddPanelToTeam(Panel panel, Team team)
    {
        if (team.Panels.Contains(panel))
            throw new PanelNotValidException("Panel is already in team");

        if (team.Panels.Any(p => p.Name.Equals(panel.Name)))
            throw new ArgumentException("A panel with the same name already exists in the team.");

        return true;
    }

    private bool CanRemovePanelFromTeam(Panel panel, Team team)
    {
        if (!team.Panels.Contains(panel))
            throw new PanelNotValidException("Panel is not in team");

        return true;
    }

    private bool IsTeamNameUnique(string teamName)
    {
        return _teamRepository.GetAllTeams().All(t => t.Name != teamName);
    }

    private bool IsTeamFull(Team team)
    {
        return team.Users.Count >= team.MaxAmountOfMembers;
    }
}