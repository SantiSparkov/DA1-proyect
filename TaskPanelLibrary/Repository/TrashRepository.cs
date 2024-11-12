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

    public Trash AddTrash(Trash trash)
    {
        _trashes.Add(trash);
        return trash;
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
        throw new TrashNotValidException(id);
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
        throw new TrashNotValidException(id);
    }

    public int Count()
    {
        return _trashes.Count;
    }

    public Trash UpdateTrash(Trash trash)
    {
        for (int i = 0; i < _trashes.Count; i++)
        {
            if (_trashes[i].Id == trash.Id)
            {
                _trashes[i] = trash;
                return trash;
            }
        }
        throw new TrashNotValidException(trash.Id);
    }

    public List<Trash> GetAll()
    {
        return _trashes;
    }
}