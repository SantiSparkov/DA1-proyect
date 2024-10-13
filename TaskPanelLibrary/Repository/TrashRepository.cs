using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Exception;
using TaskPanelLibrary.Repository.Interface;

namespace TaskPanelLibrary.Repository;

public class TrashRepository : ITrashRepository
{
    private readonly List<Trash> _trashes;

    public TrashRepository()
    {
        _trashes = new List<Trash>();
    }

    public void AddTrash(Trash trash)
    {
        _trashes.Add(trash);
    }

    public Trash GetTrashById(int id)
    {
        foreach (var trash in _trashes)
        {
            if (trash.Id == id)
            {
                return trash;
            }
        }
        throw new TaskPanelException($"Trash with id: {id} do no exist");
    }

    public Trash DeleteTrashForId(int id)
    {
        
        foreach (var trash in _trashes)
        {
            if (trash.Id == id)
            {
                _trashes.Remove(trash);
                return trash;
            }
        }
        throw new TaskPanelException($"Trash with id: {id} do no exist");
    }

    public int Count()
    {
        return _trashes.Count;
    }

    public List<Trash> GetAll()
    {
        return _trashes;
    }
}