using TaskPanelLibrary.Entity;
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
            Id = 1
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
            Id = 1
        };

        //Act
        _trashRepository.AddTrash(trash);
        Trash trashSaved = _trashRepository.GetTrashById(trash.Id);

        //Assert
        Assert.AreEqual(trash.Id, trashSaved.Id);
    }
    
    [TestMethod]
    public void DeleteTrashNoExistTest()
    {
        //Arrange
        Trash trash = new Trash()
        {
            Id = 1
        };
        _trashRepository.AddTrash(trash);
        Trash trash2 = new Trash()
        {
            Id = 2
        };
        _trashRepository.AddTrash(trash2);

        //Act
        var exception = Assert.ThrowsException<TrashNotValidException>(() => _trashRepository.DeleteTrashForId(3));
        
        // Assert
        Assert.AreEqual($"Trash with id {3} not found", exception.Message);
    }
    
    [TestMethod]
    public void DeleteTrashTest()
    {
        //Arrange
        Trash trash = new Trash()
        {
            Id = 1
        };
        _trashRepository.AddTrash(trash);
        Trash trash2 = new Trash()
        {
            Id = 2
        };
        _trashRepository.AddTrash(trash2);

        //Act
        _trashRepository.DeleteTrashForId(trash.Id);
        var exception = Assert.ThrowsException<TrashNotValidException>(() => _trashRepository.GetTrashById(trash.Id));
        
        // Assert
        Assert.AreEqual($"Trash with id {trash.Id} not found", exception.Message);
    }
    
    [TestMethod]
    public void UpdateTrash()
    {
        //Arrange
        Trash trash3 = new Trash()
        {
            Id = 3
        };
        _trashRepository.AddTrash(trash3);
        
        Trash trash = new Trash()
        {
            Id = 1
        };
        _trashRepository.AddTrash(trash);
        
        //Act
        trash.Id = 2;
        _trashRepository.UpdateTrash(trash);
        Trash trashSaved = _trashRepository.GetTrashById(trash.Id);        
        // Assert
        Assert.AreEqual(2, trashSaved.Id);
    }

    [TestMethod]
    public void UpdateTrashNoExist()
    {
        //Arrange
        Trash trash3 = new Trash()
        {
            Id = 3
        };
        _trashRepository.AddTrash(trash3);
        
        Trash trash = new Trash()
        {
            Id = 1
        };
        
        //Act
        trash.Id = 2;
        var exception = Assert.ThrowsException<TrashNotValidException>(() => _trashRepository.UpdateTrash(trash));

        // Assert
        Assert.AreEqual($"Trash with id {trash.Id} not found", exception.Message);
    }


    [TestCleanup]
    public void Cleanup()
    {
        _trashRepository.GetAllTrash().Clear();
    }
    
}