using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Entity.Enum;
using TaskPanelLibrary.Exception;
using TaskPanelLibrary.Repository;
using TaskPanelLibrary.Repository.Interface;
using TaskPanelLibrary.Service;
using TaskPanelLibrary.Service.Interface;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelTest.ServiceTest;
[TestClass]
public class CommentServiceTest
{
    private ICommentService _commentService;

    private PanelRepository _panelRepository;

    private IPanelService _panelService;

    private User _user;

    private Task _task;
    
    [TestInitialize]
    public void Initialize()
    {
        //Arrange
        _panelRepository = new PanelRepository();
        _panelService = new PanelService(_panelRepository);
        _commentService = new CommentService();
        
        _task = new Task()
        {
            Id = 1,
            PanelId = 1,
            Description = "Description test",
            Priority = TaskPriority.LOW

        };
        _user = new User()
        {
            Name = "User Manager",
            Id = 1,
            IsAdmin = true,
            Email = "prueba@hotmail.com"
        };
    }

    [TestMethod]
    public void AddComment()
    {
        //Arrange
        Panel panel = _panelService.CreatePanel(_user);
        Comment comment = new Comment()
        {
            Id = 1,
            Message = "Comment test",
            Status = Comment.EStatus.PENDING
        };
        _panelService.AddTask(panel.Id, _task);
        
        //Act
        _commentService.AddComment(_task, comment);
        Comment commentSaved = _commentService.FindById(panel,comment.Id);

        //Assert
        Assert.AreEqual(comment.Id, commentSaved.Id);
        Assert.AreEqual(comment.Message, commentSaved.Message);
        Assert.AreEqual(comment.Status, commentSaved.Status);
    }
    
    [TestMethod]
    public void FindByIdNotExist()
    {
        //Arrange
        Panel panel = _panelService.CreatePanel(_user);

        //Act
        var exception= Assert.ThrowsException<TaskPanelException>(() =>_commentService.FindById(panel,12));

        //Assert
        Assert.AreEqual("Comment do not exist", exception.Message);
    }
    
    [TestMethod]
    public void DeleteComment()
    {
        //Arrange
        Panel panel = _panelService.CreatePanel(_user);
        Comment comment = new Comment()
        {
            Id = 1,
            Message = "Comment test",
            Status = Comment.EStatus.PENDING
        };
        
        //Act
        _commentService.AddComment(_task, comment);
        Comment commentDeleted = _commentService.DeleteComment(_task, comment);
        var exception= Assert.ThrowsException<TaskPanelException>(() =>_commentService.FindById(panel,commentDeleted.Id));
        
        //Assert
        Assert.AreEqual(comment.Message, commentDeleted.Message);
        Assert.AreEqual(comment.Id, commentDeleted.Id);
        Assert.AreEqual(comment.Status, commentDeleted.Status);
        Assert.AreEqual("Comment do not exist", exception.Message);
    }
    
    [TestMethod]
    public void UpdateComment()
    {
        //Arrange
        Panel panel = _panelService.CreatePanel(_user);
        Comment comment = new Comment()
        {
            Id = 1,
            Message = "Comment test",
            Status = Comment.EStatus.PENDING
        };
        
        Comment commentToUpdate = new Comment()
        {
            Id = 1,
            Message = "Comment update",
            Status = Comment.EStatus.RESOLVED,
            ResolvedBy = _user,
            ResolvedAt = new DateTime(2008, 6, 1, 7, 47, 0)
        };
        //Act
        _commentService.AddComment(_task, comment);
        _commentService.UpdateComment(_task, commentToUpdate);
        Comment comentUpdated = _commentService.FindById(panel, comment.Id);
        
        //Assert
        Assert.AreEqual(comentUpdated.Message, commentToUpdate.Message);
        Assert.AreEqual(comentUpdated.Id, commentToUpdate.Id);
        Assert.AreEqual(comentUpdated.Status, commentToUpdate.Status);
        Assert.AreEqual(comentUpdated.ResolvedAt, commentToUpdate.ResolvedAt);
        Assert.AreEqual(comentUpdated.ResolvedBy, commentToUpdate.ResolvedBy);

    }
}