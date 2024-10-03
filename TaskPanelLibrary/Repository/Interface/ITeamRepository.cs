using Team = TaskPanelLibrary.Entity.Team;

namespace TaskPanelLibrary.Repository.Interface;

public interface ITeamRepository
{
    Team GetTeamById(int id);
    
    List<Team> GetAllTeams();
    
    Team AddTeam(Team team);
    
    Team UpdateTeam(Team team);
    
    Team DeleteTeam(int id);
}