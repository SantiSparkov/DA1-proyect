namespace TaskPanelLibrary.Exception.Team;

public class TeamNotValidException : SystemException
{
    public TeamNotValidException (int teamId) 
        : base($"Team with id {teamId} not found")
    {
    }
    
    public TeamNotValidException (string message) 
        : base(message)
    {
    }
}