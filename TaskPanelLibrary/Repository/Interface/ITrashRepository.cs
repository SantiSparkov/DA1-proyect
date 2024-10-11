using TaskPanelLibrary.Entity;

namespace TaskPanelLibrary.Repository.Interface;

public interface ITrashRepository
{
    public void AddTrash(Trash trash);

    public Trash GetTrashById(int id);

    public void DeleteTrashForId(int id);

    public int Count();

    public List<Trash> GetAll();
}