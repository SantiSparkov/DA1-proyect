using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Exception.Panel;
using TaskPanelLibrary.Exception.Task;
using TaskPanelLibrary.Exception.Team;
using TaskPanelLibrary.Exception.User;
using TaskPanelLibrary.Repository;
using DateTime = System.DateTime;
using TaskPanelLibrary.Repository.Interface;
using TaskPanelLibrary.Service.Interface;
using Team = TaskPanelLibrary.Entity.Team;

namespace TaskPanelLibrary.Service;

public class TeamService : ITeamService
{
    private readonly ITeamRepository _teamRepository;

    private readonly IUserService _userService;
    
    public TeamService(ITeamRepository teamRepository, IUserService userService)
    {
        _teamRepository = teamRepository;
        _userService = userService;
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
            MaxAmountOfMembers = team.MaxAmountOfMembers == 1 ? 2 : team.MaxAmountOfMembers,
            TeamLeaderId = user.Id,
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
    
    public List<Team> TeamsForUser(int userId)
    {
        return _teamRepository.GetAllTeams()
            .Where(team => team.Users.Any(user => user.Id == userId))
            .ToList();
    }
    
    private bool CanUpdateTeam(User updater, Team updatedTeam)
    {
        Team existingTeam = _teamRepository.GetTeamById(updatedTeam.Id);

        if (!updater.IsAdmin || updater.Id != existingTeam.TeamLeaderId)
            throw new UserNotValidException("User is not admin or team leader.");

        if (string.IsNullOrEmpty(updatedTeam.Name) && updatedTeam.Name != existingTeam.Name)
            throw new TeamNotValidException("Invalid or duplicate team name.");

        if (updatedTeam.MaxAmountOfMembers <= 0)
            throw new TeamNotValidException("Invalid maximum number of users. Must be greater than zero.");

        if (updatedTeam.MaxAmountOfMembers < existingTeam.Users.Count)
            throw new TeamNotValidException(
                "Cannot set the maximum number of users lower than the current number of users.");

        if (string.IsNullOrEmpty(updatedTeam.TasksDescription))
            throw new ArgumentException("Tasks description cannot be null or empty.");

        return true;
    }

    private bool CanDeleteTeam(Team team, User user)
    {
        if (team.Panels.Count > 0)
            throw new TeamNotValidException("Team cannot be deleted because it has associated panels.");

        if (!user.IsAdmin)
            throw new UserNotValidException("Only an administrator can delete this team.");

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

    private bool IsTeamNameUnique(string teamName)
    {
        return _teamRepository.GetAllTeams().All(t => t.Name != teamName);
    }
}