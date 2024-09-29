using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Repository.Interface;

namespace TaskPanelLibrary.Repository;

public class PanelRepository : IPanelRepository
{
    private List<Panel> _panels;

    public PanelRepository()
    {
        _panels = new List<Panel>();
    }
    
    public Panel add(Panel panel)
    {
        this._panels.Add(panel);
        return panel;
    }

    public Panel delete(int id)
    {
        foreach (var panel in _panels)
        {
            if (panel.Id.Equals(id))
            {
                _panels.Remove(panel);
            }
        }
        throw new ArgumentException("Panel does not exist");
    }

    public Panel findById(int id)
    {
        foreach (var panel in _panels)
        {
            if (panel.Id.Equals(id))
            {
                return panel;
            }
        }
        throw new ArgumentException("Panel does not exist");
    }

    public Panel update(Panel panel)
    {
        return new Panel();
    }

    public List<Panel> getAll()
    {
        return _panels;
    }
}