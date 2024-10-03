using TaskPanelLibrary.Exception.Team;
using TaskPanelLibrary.Repository.Interface;
using Team = TaskPanelLibrary.Entity.Team;

namespace TaskPanelLibrary.Repository;

public class TeamRepository : ITeamRepository
{
    private readonly List<Team> _teams = new List<Team>();
    
    public Team GetTeamById(int id)
    {
        var team = _teams.FirstOrDefault(t => t.Id == id)
                   ?? throw new TeamNotFoundException(id);
        return team;
    }

    public List<Team> GetAllTeams()
    {
        var teams = _teams;
        return teams;
    }

    public Team AddTeam(Team team)
    {
        team.Id = _teams.Count > 0 ? _teams.Max(t => t.Id) + 1 : 1;
        _teams.Add(team);
        return team;
    }

    public Team UpdateTeam(Team team)
    {
        var existingTeam = _teams.FirstOrDefault(t => t.Id == team.Id)
                           ?? throw new TeamNotFoundException(team.Id);
        existingTeam.Name = team.Name ?? existingTeam.Name;
        existingTeam.CreationDate = team.CreationDate != default(DateTime) ? team.CreationDate : existingTeam.CreationDate;
        existingTeam.TasksDescription = team.TasksDescription ?? existingTeam.TasksDescription;
        existingTeam.MaxAmountOfMembers = team.MaxAmountOfMembers != default(int) ? team.MaxAmountOfMembers : existingTeam.MaxAmountOfMembers;
        existingTeam.Users = team.Users ?? existingTeam.Users;
        return existingTeam;
    }

    public Team DeleteTeam(int id)
    {
        var team = _teams.FirstOrDefault(t => t.Id == id)
                   ?? throw new TeamNotFoundException(id);
        _teams.Remove(team);
        return team;
    }
}