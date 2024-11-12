using TaskPanelLibrary.Config;
using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Repository.Interface;

namespace TaskPanelLibrary.Repository;

public class TrashSqlRepository : ITrashRepository
{
    private SqlContext _trashDatabase;

    public TrashSqlRepository(SqlContext sqlContext)
    {
        _trashDatabase = sqlContext;
    }

    public Trash AddTrash(Trash trash)
    {
        _trashDatabase.Trashes.Add(trash);
        _trashDatabase.SaveChanges();
        
        return trash;
    }

    public Trash GetTrashById(int id)
    {
        Trash trash = _trashDatabase.Trashes.Find(id);
        if (trash == null)
        {
            throw new System.Exception($"Trash with id: {id} does not exist");
        }

        return trash;
    }

    public Trash DeleteTrashForId(int id)
    {
        Trash trash = _trashDatabase.Trashes.Find(id);
        if (trash == null)
        {
            throw new System.Exception($"Trash with id: {id} does not exist");
        }

        _trashDatabase.Trashes.Remove(trash);
        _trashDatabase.SaveChanges();
        return trash;
    }

    public int Count()
    {
        return _trashDatabase.Trashes.Count();
    }

    public Trash UpdateTrash(Trash trash)
    {
        _trashDatabase.Trashes.Update(trash);
        _trashDatabase.SaveChanges();
        
        return trash;
    }

    public List<Trash> GetAll()
    {
        return _trashDatabase.Trashes.ToList();
    }
}