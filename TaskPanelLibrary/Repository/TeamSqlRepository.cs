using Microsoft.EntityFrameworkCore;
using TaskPanelLibrary.Config;
using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Repository.Interface;

namespace TaskPanelLibrary.Repository;

public class TeamSqlRepository : ITeamRepository
{
    private readonly SqlContext _teamDataBase;

    public TeamSqlRepository(SqlContext sqlContext)
    {
        _teamDataBase = sqlContext;
    }

    public Team GetTeamById(int id)
    {
        Team team = _teamDataBase.Teams.Find(id);
        if (team == null)
        {
            throw new System.Exception($"Team with id: {id} does not exist");
        }

        return team;
    }

    public List<Team> GetAllTeams()
    {
        return _teamDataBase.Teams
            .Include(team => team.Users)
            .ToList();
    }

    public Team AddTeam(Team team)
    {
        _teamDataBase.Teams.Add(team);
        _teamDataBase.SaveChanges();
        return team;
    }

    public Team UpdateTeam(Team team)
    {
        _teamDataBase.Teams.Update(team);
        return team;
    }

    public Team DeleteTeam(int id)
    {
        Team team = _teamDataBase.Teams.Find(id);
        if (team == null)
        {
            throw new System.Exception($"Team with id: {id} does not exist");
        }

        _teamDataBase.Teams.Remove(team);
        _teamDataBase.SaveChanges();
        return team;
    }
}