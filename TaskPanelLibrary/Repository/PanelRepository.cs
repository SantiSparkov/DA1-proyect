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
        panel.Id = _panels.Count > 0 ? _panels.Max(t => t.Id) + 1 : 1;
        _panels.Add(panel);
        Console.WriteLine($"Panel {panel.Name} added successfully");
        return panel;
    }
    
    public Panel DeletePanel(int id)
    {
        var panel = _panels.FirstOrDefault(t => t.Id == id)
                    ?? throw new ArgumentException("Panel does not exist");
        
        _panels.Remove(panel);
        return panel;
    }

    public Panel GetPanelById(int id)
    {
        var panel = _panels.FirstOrDefault(t => t.Id == id)
                   ?? throw new ArgumentException("Panel does not exist");
        return panel;
    }

    public Panel UpdatePanel(Panel panel)
    {
        Panel panelSaved = GetPanelById(panel.Id);
        
        panelSaved.Description = panel.Description;
        panelSaved.Name = panel.Name;
        panelSaved.Team = panel.Team;
        
        return panelSaved;
    }

    public List<Panel> GetAllPanels()
    {
        return _panels;
    }

    public int Count()
    {
        return _panels?.Count ?? 0;
    }
}