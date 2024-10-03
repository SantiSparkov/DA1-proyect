namespace TaskPanelLibrary.Exception.Team;

public class TeamNotFoundException : SystemException
{
    public TeamNotFoundException (int teamId) 
        : base($"Team with id {teamId} not found")
    {
    }
}