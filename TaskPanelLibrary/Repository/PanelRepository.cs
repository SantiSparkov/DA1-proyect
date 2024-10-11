using Microsoft.VisualBasic.CompilerServices;
using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Repository.Interface;

namespace TaskPanelLibrary.Repository;

public class PanelRepository : IPanelRepository
{
    private readonly List<Panel> _panels;

    public PanelRepository()
    {
        _panels = new List<Panel>();
    }

    public Panel AddPanel(Panel panel)
    {
        //Verificar datos a guardar
        this._panels.Add(panel);
        return panel;
    }
    
    public Panel Delete(int id)
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

    public Panel FindById(int id)
    {
        List<Panel> panels = _panels.Where(p => p.Id == id).ToList();

        if (panels == null || panels.Count == 0)
        {
            throw new ArgumentException("Panel does not exist");
        }

        return panels[0];
    }

    public Panel Update(Panel panel)
    {
        Panel panelSaved = FindById(panel.Id);
        panelSaved.Description = panel.Description;
        panelSaved.Name = panel.Name;
        return panelSaved;
    }

    public List<Panel> GetAll()
    {
        return _panels;
    }

    public int Count()
    {
        return _panels?.Count ?? 0;
    }
}