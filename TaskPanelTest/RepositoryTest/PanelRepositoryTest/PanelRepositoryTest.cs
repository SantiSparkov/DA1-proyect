using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Repository.Interface;

namespace TaskPanelTest.RepositoryTest.PanelRepositoryTest;

[TestClass]
public class PanelRepositoryTest
{
    private IPanelRepository _panelRepository;
    
    [TestInitialize]
    public void Initialize()
    {   //Arrange
        _panelRepository = new PanelRepository();
    }
    
    [TestMethod]
    public void CreatePanelRepository()
    {
        //Arrange 
        //Act 
        // Assert
        Assert.IsNotNull(_panelRepository);
    }
    
    [TestMethod]
    public void PanelRepository_addPanel()
    {
        //Arrange 
        Panel panel = new Panel();
        
        //Act 
        _panelRepository.add(panel);
        Panel panelSave = _panelRepository.findById(panel.Id);
        
        // Assert
        Assert.Equals(panel.Id, panelSave.Id);
    }

    [TestMethod] 
    public void PanelRepositoryDeletePanel()
    {
        //Arrange 
        Panel panel = new Panel();
        
        //Act 
        _panelRepository.add(panel);
        var exception = Assert.ThrowsException<System.ArgumentException>(() => _panelRepository.findById(panel.Id));
        
        // Assert
        Assert.AreEqual("Panel does not exist", exception.Message);
    }
    
    [TestMethod] 
    public void PanelRepositoryUpdatePanel()
    {
        //Arrange 
        Panel panel = new Panel()
        {
            Id = 12,
            Name = "Panel test",
            Description = "Description test"
        };
        _panelRepository.add(panel);
        panel.Description = "Update description";
        panel.Name = "Update panel test";

        //Act 
        _panelRepository.update(panel);
        Panel panelUpdated = _panelRepository.findById(panel.Id);
        
        // Assert
        Assert.AreEqual("Update description", panelUpdated.Description);
        Assert.AreEqual("Update panel test", panelUpdated.Name);
    }
    
    [TestCleanup]
    public void Cleanup()
    {
        _panelRepository.getAll().Clear();
    }
}