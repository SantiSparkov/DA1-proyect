namespace TaskPanelLibrary.Exception.Panel;

public class PanelNotValidException : SystemException
{
    public PanelNotValidException(int id)
        : base($"Panel with id {id} not found")
    {
    }
    
    public PanelNotValidException(string message)
        : base(message)
    {
    }
}