using TaskPanelLibrary.Config;
using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Exception;
using TaskPanelLibrary.Repository.Interface;

namespace TaskPanelLibrary.Repository;

public class EpicSqlRepository : IEpicRepository
{
    private readonly SqlContext _epicDataBase;

    public EpicSqlRepository(SqlContext epicDataBase)
    {
        _epicDataBase = epicDataBase;
    }
    
    public Epic AddEpic(Epic epic)
    {
        _epicDataBase.Epics.Add(epic);
        _epicDataBase.SaveChanges();
        return epic;
    }

    public Epic DeleteEpic(int id)
    {
        Epic epicSaved = _epicDataBase.Epics.Find(id);
        if (epicSaved != null)
        {
            _epicDataBase.Epics.Remove(epicSaved);
            _epicDataBase.SaveChanges();
            return epicSaved;
        }
        
        throw new EpicNotValidException($"Epic with id: {id} does not exist");
    }

    public Epic GetEpicById(int id)
    {
        Epic epic = _epicDataBase.Epics.Find(id);
        if (epic == null)
        {
            throw new EpicNotValidException($"Epic with id: {id} does not exist");
        }

        return epic;
    }

    public List<Epic> GetAllEpics()
    {
        return _epicDataBase.Epics.ToList();
    }

    public Epic UpdateEpic(Epic epic)
    {
        _epicDataBase.Update(epic);
        _epicDataBase.SaveChanges();
        return epic;
    }

    public int Count()
    {
        return _epicDataBase.Epics.Count();
    }
}