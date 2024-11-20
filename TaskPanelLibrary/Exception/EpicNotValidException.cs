namespace TaskPanelLibrary.Exception;

public class EpicNotValidException : SystemException
{
    public EpicNotValidException(string message)
        : base(message)
    {
    }
}
    