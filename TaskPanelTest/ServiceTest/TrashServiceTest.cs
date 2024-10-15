using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Exception;
using TaskPanelLibrary.Repository;
using TaskPanelLibrary.Service;
using TaskPanelLibrary.Service.Interface;

namespace TaskPanelTest.ServiceTest;

[TestClass]
public class TrashServiceTest
{
    private ITrashService _trashService;

    private TrashRepository _trashRepository;

    
    [TestInitialize]
    public void Initialize()
    {
        //Arrange
        _trashRepository = new TrashRepository();
        _trashService = new TrashService(_trashRepository);
    }
    
    [TestMethod]
    public void CreateTrashTest()
    {
        //Arrange
        Trash trash = _trashService.CreateTrash();

        //Act

        //Assert
        Assert.IsNotNull(trash);
        Assert.AreEqual(1, trash.Id);
    }
    
    [TestMethod]
    public void GetTrashById()
    {
        //Arrange
        Trash trash = _trashService.CreateTrash();

        //Act
        Trash trashSaved = _trashService.GetTrashById(trash.Id);
        //Assert
        Assert.IsNotNull(trashSaved);
        Assert.AreEqual(trash.Id, trashSaved.Id);
    }
    
    [TestMethod]
    public void DeleteTrashTest()
    {
        //Arrange
        Trash trash = _trashService.CreateTrash();

        //Act
        _trashService.DeleteTrash(trash.Id);
        var exception = Assert.ThrowsException<TrashNotValidException>(() => _trashService.GetTrashById(trash.Id));
        
        // Assert
        Assert.AreEqual($"Trash with id 1 not found", exception.Message);
    }
    
    [TestCleanup]
    public void Cleanup()
    {
        _trashRepository.GetAll().Clear();
    }
    
}