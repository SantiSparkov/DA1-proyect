using TaskPanelLibrary.Config;
using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Repository.Interface;

namespace TaskPanelLibrary.Repository;

public class PanelSqlRepository : IPanelRepository
{
    private SqlContext _panelDataBase;

    public PanelSqlRepository(SqlContext sqlContext)
    {
        _panelDataBase = sqlContext;
    }

    public Panel AddPanel(Panel panel)
    {
        _panelDataBase.Panels.Add(panel);
        _panelDataBase.SaveChanges();
        return panel;
    }

    public Panel DeletePanel(int id)
    {
        Panel panel = _panelDataBase.Panels.Find(id);
        if (panel == null)
        {
            throw new System.Exception($"Panel with id: {id} does not exist");
        }

        _panelDataBase.Panels.Remove(panel);
        _panelDataBase.SaveChanges();
        return panel;
    }

    public Panel GetPanelById(int id)
    {
        Panel panel = _panelDataBase.Panels.Find(id);
        if (panel == null)
        {
            throw new System.Exception($"Panel with id: {id} does not exist");
        }

        return panel;
    }

    public Panel UpdatePanel(Panel panel)
    {
        _panelDataBase.Panels.Update(panel);
        _panelDataBase.SaveChanges();
        return panel;
    }

    public List<Panel> GetAllPanels()
    {
        return _panelDataBase.Panels.ToList();
    }

    public int Count()
    {
        return _panelDataBase.Panels.Count();
    }
}