using TaskPanelLibrary.Entity;

namespace TaskPanelLibrary.Service.Interface;

public interface ITrashService
{
    public Trash CreateTrash();
    
    public Trash GetTrashById(int id);

    public Trash DeleteTrash(int id);
}