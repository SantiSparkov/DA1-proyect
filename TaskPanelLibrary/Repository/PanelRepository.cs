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
            if (panel.Id == id)
            {
                _panels.Remove(panel);
                return panel;
            }
        }
        throw new ArgumentException("Panel does not exist");
    }

    public Panel findById(int id)
    {
        List<Panel> panels = _panels.Where(p => p.Id == id).ToList();

        if (panels == null || panels.Count == 0)
        {
            throw new ArgumentException("Panel does not exist");
        }

        return panels[0];
    }

    public Panel update(Panel panel)
    {
        Panel panelSaved = findById(panel.Id);
        panelSaved.Description = panel.Description;
        panelSaved.Name = panel.Name;
        return panelSaved;
    }

    public List<Panel> getAll()
    {
        return _panels;
    }
}