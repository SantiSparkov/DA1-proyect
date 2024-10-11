using TaskPanelLibrary.Repository;
using TaskPanelLibrary.Service.Interface;
using Team = TaskPanelLibrary.Entity.Task;

namespace TaskPanelLibrary.Service;

public class TeamService : ITeamService
{
    private readonly TeamRepository _teamRepository;
    private UserService _userService;
    private PanelService _panelService;
    
    public TeamService(TeamRepository teamRepository, UserService userService, PanelService panelService)
    {
        _teamRepository = teamRepository;
        _userService = userService;
        _panelService = panelService;
    }
    
    public Team CreateTeam(Team team, int userId)
    {
        throw new NotImplementedException();
    }

    public Team DeleteTeam(Team team, int userId)
    {
        throw new NotImplementedException();
    }

    public Team UpdateTeam(Team team, int userId)
    {
        throw new NotImplementedException();
    }

    public Team GetTeamById(int id)
    {
        throw new NotImplementedException();
    }

    public List<Team> GetAllTeams()
    {
        throw new NotImplementedException();
    }

    public void AddUserToTeam(int userId, Team team)
    {
        throw new NotImplementedException();
    }

    public void RemoveUserFromTeam(int userId, Team team)
    {
        throw new NotImplementedException();
    }

    public void AddPanelToTeam(int panelId, Team team)
    {
        throw new NotImplementedException();
    }

    public void RemovePanelFromTeam(int panelId, Team team)
    {
        throw new NotImplementedException();
    }
}