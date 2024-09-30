using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Repository;
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
        Panel panel = new Panel()
        {
            Id = 1234
        };
        
        //Act 
        _panelRepository.add(panel);
        Panel panelSaved = _panelRepository.findById(panel.Id);
        
        // Assert
        Assert.AreEqual(panel.Id, panelSaved.Id);
    }

    [TestMethod] 
    public void PanelRepositoryDeletePanel()
    {
        //Arrange 
        Panel panel = new Panel()
        {
            Id = 123
        };
        
        //Act 
        _panelRepository.add(panel);
        Panel panelRemoved = _panelRepository.delete(panel.Id);
        // Assert
        Assert.AreEqual(0, _panelRepository.getAll().Count);
    }
    
    [TestMethod] 
    public void PanelRepositoryNotExistDeletePanel()
    {
        //Arrange 
        
        //Act 
        var exception = Assert.ThrowsException<System.ArgumentException>(() => _panelRepository.delete(12345));
        
        // Assert
        Assert.AreEqual("Panel does not exist", exception.Message);
    }
    
    [TestMethod] 
    public void PanelRepositoryNotExistPanel()
    {
        //Arrange 
        Panel panel = new Panel()
        {
            Id = 123
        };
        Panel panel2 = new Panel()
        {
            Id = 1234
        };
        
        //Act 
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