using TaskPanelLibrary.Entity;

namespace TaskPanelLibrary.Repository.Interface;

public interface ITrashRepository
{
    public Trash AddTrash(Trash trash);

    public Trash GetTrashById(int id);

    public Trash DeleteTrashForId(int id);
    
    public Trash UpdateTrash(Trash trash);
}