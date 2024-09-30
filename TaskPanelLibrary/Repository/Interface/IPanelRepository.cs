using TaskPanelLibrary.Entity;

namespace TaskPanelLibrary.Repository.Interface;

public interface IPanelRepository
{ 
    public Panel add(Panel panel);

    public Panel delete(int id);

    public Panel findById(int id);

    public Panel update(Panel panel);

    public List<Panel> getAll();
}