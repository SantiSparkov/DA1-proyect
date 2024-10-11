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

    private CommentRepository _commentRepository;


    private IPanelService _panelService;
    
    private User _user;

    private Task _task;
    
    [TestInitialize]
    public void Initialize()
    {
        //Arrange
        _commentRepository = new CommentRepository();
        _commentService = new CommentService(_commentRepository);
        
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
        Comment comment = new Comment()
        {
            Id = 14,
            Message = "Comment test",
            Status = EStatusComment.PENDING
        };

        //Act
        _task.CommentList.Add(comment);
        Comment commentSaved = _commentService.FindById(_task,comment.Id);

        //Assert
        Assert.AreEqual(comment.Id, commentSaved.Id);
        Assert.AreEqual(comment.Message, commentSaved.Message);
        Assert.AreEqual(comment.Status, commentSaved.Status);
    }
    
    [TestMethod]
    public void FindByIdNotExist()
    {
        //Arrange
        //Act
        var exception= Assert.ThrowsException<TaskPanelException>(() =>_commentService.FindById(_task,12));

        //Assert
        Assert.AreEqual("Comment with id: 12 do not exist", exception.Message);
    }
    
    [TestMethod]
    public void DeleteComment()
    {
        //Arrange
        Comment comment = new Comment()
        {
            Id = 15,
            Message = "Comment test",
            Status = EStatusComment.PENDING
        };
        
        //Act
        _task.CommentList.Add(comment);
        Comment commentDeleted = _commentService.DeleteComment(_task, comment);
        var exception= Assert.ThrowsException<TaskPanelException>(() =>_commentService.FindById(_task,commentDeleted.Id));
        
        //Assert
        Assert.AreEqual(comment.Message, commentDeleted.Message);
        Assert.AreEqual(comment.Id, commentDeleted.Id);
        Assert.AreEqual(comment.Status, commentDeleted.Status);
        Assert.AreEqual($"Comment with id: {commentDeleted.Id} do not exist", exception.Message);
    }
    
    [TestMethod]
    public void UpdateComment()
    {
        //Arrange
        Comment comment = new Comment()
        {
            Id = 1,
            Message = "Comment test",
            Status = EStatusComment.PENDING
        };
        
        Comment commentToUpdate = new Comment()
        {
            Id = 1,
            Message = "Comment update",
            Status = EStatusComment.RESOLVED,
            ResolvedBy = _user,
            ResolvedAt = new DateTime(2008, 6, 1, 7, 47, 0)
        };
        //Act
        _task.CommentList.Add(comment);
        _commentService.UpdateComment(_task, commentToUpdate);
        Comment comentUpdated = _commentService.FindById(_task, comment.Id);
        
        //Assert
        Assert.AreEqual(comentUpdated.Message, commentToUpdate.Message);
        Assert.AreEqual(comentUpdated.Id, commentToUpdate.Id);
        Assert.AreEqual(comentUpdated.Status, commentToUpdate.Status);
        Assert.AreEqual(comentUpdated.ResolvedAt, commentToUpdate.ResolvedAt);
        Assert.AreEqual(comentUpdated.ResolvedBy, commentToUpdate.ResolvedBy);

    }
}