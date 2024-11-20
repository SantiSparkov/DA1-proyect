using TaskPanelLibrary.Entity;

namespace TaskPanelLibrary.Repository.Interface;

public interface IEpicRepository
{
    public Epic AddEpic(Epic epic);
    
    public Epic DeleteEpic(int id);
    
    public Epic GetEpicById(int id);
    
    public List<Epic> GetAllEpics();
    
    public Epic UpdateEpic(Epic epic);
    
    public int Count();
}