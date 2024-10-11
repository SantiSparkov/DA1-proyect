namespace TaskPanelLibrary.Exception.Panel;

public class PanelNotFoundException : SystemException
{
    public PanelNotFoundException(int id)
        : base($"Panel with id {id} not found")
    {
    }
}