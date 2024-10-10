namespace TaskPanelLibrary.Exception;

public class ApiException : System.Exception
{
    public ApiException(string message): base(message)
    {
    }
}