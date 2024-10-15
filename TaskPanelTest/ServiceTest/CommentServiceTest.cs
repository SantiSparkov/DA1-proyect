using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Entity.Enum;
using TaskPanelLibrary.Exception.Comment;
using TaskPanelLibrary.Repository;
using TaskPanelLibrary.Repository.Interface;
using TaskPanelLibrary.Service;
using TaskPanelLibrary.Service.Interface;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelTest.ServiceTest;
[TestClass]
public class CommentServiceTest
{
    private ICommentRepository _commentRepository;
    
    private IUserRepository _userRepository;
    
    private ICommentService _commentService;
    
    private IUserService _userService;
    
    private PasswordGeneratorService _passwordGeneratorService;
    
    private User _user;

    private Task _task;
    
    private Comment _comment;
    
    [TestInitialize]
    public void Initialize()
    {
        //Arrange
        _commentRepository = new CommentRepository();
        _userRepository = new UserRepository();
        _userService = new UserService(_userRepository, _passwordGeneratorService);
        _commentService = new CommentService(_commentRepository,_userService); 
        
        _task = new Task()
        {
            Id = 1,
            PanelId = 1,
            Description = "Description test",
            Priority = ETaskPriority.LOW

        };
        _user = new User()
        {
            Name = "User Manager",
            Id = 1,
            IsAdmin = true,
            Email = "prueba@hotmail.com"
        };
        _comment = new Comment()
        {
            Id = 1,
            Message = "Comment test",
            Status = EStatusComment.PENDING
        };
    }

    [TestMethod]
    public void AddComment()
    {
        //Arrange
        Comment createdComment = _commentService.CreateComment(_comment);
        createdComment.Message = "Comment test";
        createdComment.Status = EStatusComment.PENDING;

        //Act
        _task.CommentList.Add(createdComment);
        Comment commentSaved = _commentService.FindById(createdComment.Id);

        //Assert
        Assert.AreEqual(createdComment.Id, commentSaved.Id);
        Assert.AreEqual(createdComment.Message, commentSaved.Message);
        Assert.AreEqual(createdComment.Status, commentSaved.Status);
    }
    
    [TestMethod]
    public void FindByIdNotExist()
    {
        //Arrange
        //Act
        var exception= Assert.ThrowsException<CommentNotValidException>(() =>_commentService.FindById(12));

        //Assert
        Assert.AreEqual("Comment with id: 12 do not exist", exception.Message);
    }
    
    [TestMethod]
    public void FindById()
    {
        //Arrange
        Comment comment1 = _commentService.CreateComment(_comment);
        Comment comment2 = _commentService.CreateComment(_comment);
        comment2.Message = "test";
        comment2.Status = EStatusComment.PENDING;
        //Act
        Comment commentSaved = _commentService.FindById(comment2.Id);

        //Assert
        Assert.AreEqual(commentSaved.Id, comment2.Id);
        Assert.AreEqual(commentSaved.Message, comment2.Message);
        Assert.AreEqual(commentSaved.Status, comment2.Status);
    }
    
    [TestMethod]
    public void DeleteComment()
    {
        //Arrange
        Comment comment = _commentService.CreateComment(_comment);
        comment.Message = "Comment test";
        comment.Status = EStatusComment.PENDING;
        
        //Act
        _task.CommentList.Add(comment);
        Comment commentDeleted = _commentService.DeleteComment(_task, comment);
        var exception= Assert.ThrowsException<CommentNotValidException>(() =>_commentService.FindById(commentDeleted.Id));
        
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

        Comment commentToUpdate = _commentService.CreateComment(comment);
        commentToUpdate.Message = "Comment update";
        commentToUpdate.Status = EStatusComment.RESOLVED;
        commentToUpdate.ResolvedBy = _user;
        commentToUpdate.ResolvedAt = new DateTime(2008, 6, 1, 7, 47, 0);
    
        //Act
        _task.CommentList.Add(comment);
        _commentService.UpdateComment(commentToUpdate);
        Comment comentUpdated = _commentService.FindById(comment.Id);
        
        //Assert
        Assert.AreEqual(comentUpdated.Message, commentToUpdate.Message);
        Assert.AreEqual(comentUpdated.Id, commentToUpdate.Id);
        Assert.AreEqual(comentUpdated.Status, commentToUpdate.Status);
        Assert.AreEqual(comentUpdated.ResolvedAt, commentToUpdate.ResolvedAt);
        Assert.AreEqual(comentUpdated.ResolvedBy, commentToUpdate.ResolvedBy);

    }
}