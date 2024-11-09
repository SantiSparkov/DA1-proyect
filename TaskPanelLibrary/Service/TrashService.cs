using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Repository;
using TaskPanelLibrary.Repository.Interface;
using TaskPanelLibrary.Service.Interface;

namespace TaskPanelLibrary.Service;

public class TrashService : ITrashService
{
    private readonly TrashSqlRepository _trashRepository;

    public TrashService(TrashSqlRepository trashRepository)
    {
        _trashRepository = trashRepository;
    }

    public Trash CreateTrash()
    {
        Trash trash = new Trash()
        {
            Id = _trashRepository.Count() + 1
        };
        _trashRepository.AddTrash(trash);
        return trash;
    }

    public Trash GetTrashById(int id)
    {
        return _trashRepository.GetTrashById(id);
    }

    public Trash DeleteTrash(int id)
    {
       return _trashRepository.DeleteTrashForId(id);
    }
}