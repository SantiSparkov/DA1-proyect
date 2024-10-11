using TaskPanelLibrary.Entity;

namespace TaskPanelLibrary.Repository.Interface;

public interface IPanelRepository
{ 
    public Panel AddPanel(Panel panel);

    public Panel Delete(int id);

    public Panel FindById(int id);

    public Panel Update(Panel panel);

    public List<Panel> GetAll();

    public int Count();
}