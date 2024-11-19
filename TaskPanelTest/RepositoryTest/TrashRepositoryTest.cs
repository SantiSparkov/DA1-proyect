/*using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Exception;
using TaskPanelLibrary.Repository;
using TaskPanelLibrary.Repository.Interface;

namespace TaskPanelTest.RepositoryTest;

[TestClass]
public class TrashRepositoryTest
{
    private ITrashRepository _trashRepository;

    [TestInitialize]
    public void Initialize()
    {
        //Arrange
        _trashRepository = new TrashRepository();
    }

    [TestMethod]
    public void AddTrashTest()
    {
        //Arrange
        Trash trash = new Trash()
        {
            Id = _trashRepository.Count() + 1
        };

        //Act
        _trashRepository.AddTrash(trash);
        Trash trashSaved = _trashRepository.GetTrashById(trash.Id);

        //Assert
        Assert.AreEqual(trash.Id, trashSaved.Id);
    }
    
    [TestMethod]
    public void RecoverTrashTest()
    {
        //Arrange
        Trash trash = new Trash()
        {
            Id = _trashRepository.Count() + 1
        };

        //Act
        _trashRepository.AddTrash(trash);
        Trash trashSaved = _trashRepository.GetTrashById(trash.Id);

        //Assert
        Assert.AreEqual(trash.Id, trashSaved.Id);
    }
    
    [TestMethod]
    public void DeleteTrashTest()
    {
        //Arrange
        Trash trash = new Trash()
        {
            Id = _trashRepository.Count() + 1
        };
        _trashRepository.AddTrash(trash);
        Trash trash2 = new Trash()
        {
            Id = _trashRepository.Count() + 1
        };
        _trashRepository.AddTrash(trash2);

        //Act
        _trashRepository.DeleteTrashForId(trash.Id);
        var exception = Assert.ThrowsException<TrashNotValidException>(() => _trashRepository.GetTrashById(trash.Id));
        
        // Assert
        Assert.AreEqual($"Trash with id {trash.Id} not found", exception.Message);
    }
    
    [TestMethod]
    public void DeleteTrashTestDoNOtExist()
    {
        //Arrange
        Trash trash = new Trash()
        {
            Id = _trashRepository.Count() + 1
        };

        //Act
        _trashRepository.AddTrash(trash);
        var exception = Assert.ThrowsException<TrashNotValidException>(() => _trashRepository.DeleteTrashForId(2));
        
        // Assert
        Assert.AreEqual("Trash with id 2 not found", exception.Message);
    }
    
    
    [TestCleanup]
    public void Cleanup()
    {
        _trashRepository.GetAll().Clear();
    }
    
}*/