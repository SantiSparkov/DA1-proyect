using TaskPanelLibrary.Config;
using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Repository;
using TaskPanelLibrary.Repository.Interface;
using TaskPanelTest.ConfigTest;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelTest.RepositoryTest.SqlRepositories;

[TestClass]
public class TrashSqlRepositoryTest
{
    private SqlContext _sqlContext;

    private ITrashRepository _trashRepository;
    
    [TestInitialize]
    public void Initialize()
    {
        _sqlContext = new SqlContexTest().CreateMemoryContext();
        _trashRepository = new TrashSqlRepository(_sqlContext);
    }
    
    [TestCleanup]
    public void CleanUp()
    {
        _sqlContext?.Database.EnsureDeleted();
    }
    
    [TestMethod]
    public void AddTrash()
    {
        //Arrange
        Trash trash = new Trash()
        {
            Id = 1,
            Elements = 12,
            PanelList = new List<Panel>(),
            TaskList = new List<Task>(),
            UserId = 1
        };

        //Act
        _trashRepository.AddTrash(trash);

        //Assert
        Assert.AreEqual(1, _trashRepository.GetAllTrash().Count);
    }
    
    [TestMethod]
    public void GetTrashById()
    {
        //Arrange
        Trash trash = new Trash()
        {
            Id = 1,
            Elements = 12,
            PanelList = new List<Panel>(),
            TaskList = new List<Task>(),
            UserId = 1
        };

        //Act
        _trashRepository.AddTrash(trash);

        Trash trashSaved = _trashRepository.GetTrashById(1);

        //Assert
        Assert.AreEqual(trash.Id, trashSaved.Id);
        Assert.AreEqual(trash.Elements, trashSaved.Elements);
        Assert.AreEqual(trash.UserId, trashSaved.UserId);
        Assert.IsNotNull(trashSaved.PanelList);
        Assert.IsNotNull(trashSaved.TaskList);
    }
    
    [TestMethod]
    public void GetTrashByIdNotExist()
    {
        //Arrange

        //Act
        var exception = Assert.ThrowsException<System.Exception>(() => _trashRepository.GetTrashById(1));

        //Assert
        Assert.AreEqual($"Trash with id: {1} does not exist", exception.Message);
    }
    
    [TestMethod]
    public void DeleteTask()
    {
        //Arrange
        Trash trash = new Trash()
        {
            Id = 1,
            Elements = 12,
            PanelList = new List<Panel>(),
            TaskList = new List<Task>(),
            UserId = 1
        };

        //Act
        _trashRepository.AddTrash(trash);
        _trashRepository.DeleteTrashForId(trash.Id);

        //Assert
        Assert.AreEqual(0, _trashRepository.GetAllTrash().Count);
    }
    
    [TestMethod]
    public void DeleteTrashNotExist()
    {
        //Arrange

        //Act
        var exception = Assert.ThrowsException<System.Exception>(() => _trashRepository.DeleteTrashForId(1));

        //Assert
        Assert.AreEqual($"Trash with id: {1} does not exist", exception.Message);
    }
    
    [TestMethod]
    public void UpdateTrash()
    {
        //Arrange
        Trash trash = new Trash()
        {
            Id = 1,
            Elements = 12,
            PanelList = new List<Panel>(),
            TaskList = new List<Task>(),
            UserId = 1
        };

        //Act
        _trashRepository.AddTrash(trash);

        trash.Elements = 10;

        Trash trashUpdated = _trashRepository.UpdateTrash(trash);

        //Assert
        Assert.AreEqual(10, trashUpdated.Elements);
    }
    
    [TestMethod]
    public void GetAllTrash()
    {
        //Arrange
        Trash trash = new Trash()
        {
            Id = 1,
            Elements = 12,
            PanelList = new List<Panel>(),
            TaskList = new List<Task>(),
            UserId = 1
        };

        //Act
        _trashRepository.AddTrash(trash);

        List<Trash> trashes = _trashRepository.GetAllTrash();

        //Assert
        Assert.AreEqual(1, trashes.Count);
    }
}