using Team = TaskPanelLibrary.Entity.Team;
using TaskPanelLibrary.Entity;

namespace TaskPanelLibrary.Service.Interface;

public interface ITeamService
{
    public Team CreateTeam(Team team, int userId);
    
    public Team DeleteTeam(Team team , int userId);
    
    public Team UpdateTeam(Team team, int userId);
    
    public Team GetTeamById(int id);
    
    public List<Team> GetAllTeams();
    
    public void AddUserToTeam(int userId, Team team);

    public List<Team> TeamsForUser(int userId);
}