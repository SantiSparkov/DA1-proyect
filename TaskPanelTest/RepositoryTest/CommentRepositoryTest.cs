using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Entity.Enum;
using TaskPanelLibrary.Exception;
using TaskPanelLibrary.Exception.Comment;
using TaskPanelLibrary.Repository;
using TaskPanelLibrary.Repository.Interface;

namespace TaskPanelTest.RepositoryTest.CommentRepositoryTest;

[TestClass]
public class CommentRepositoryTest
{
    private ICommentRepository _commentRepository;
    
    [TestInitialize]
    public void Initialize()
    {   //Arrange
        _commentRepository = new CommentRepository();
    }
    
    [TestMethod]
    public void CreateCommentRepository()
    {
        //Arrange 
        //Act 
        // Assert
        Assert.IsNotNull(_commentRepository);
    }
    
    [TestMethod]
    public void AddCommentRepository()
    {
        //Arrange 
        User resolvedBy = new User()
        {
            Name = "Name",
            LastName = "LastName",
            Email = "email@email.com",
            BirthDate = new DateTime(2008, 6, 1, 7, 47, 0),
            Password = "password",
            IsAdmin = false,
            Trash = new Trash()
        };
        Comment comment = new Comment()
        {
            Id = 123,
            Message = "Message comment",
            ResolvedBy = resolvedBy,
            ResolvedAt = new DateTime(2008, 6, 1, 7, 47, 0),
            Status = EStatusComment.RESOLVED
        };
        
        //Act 
        _commentRepository.Add(comment);
        Comment commentSaved = _commentRepository.FindById(comment.Id);
        User user = comment.ResolvedBy;
        
        // Assert
        Assert.AreEqual(comment.Id, commentSaved.Id);
        Assert.AreEqual(comment.Message, commentSaved.Message);
        Assert.AreEqual(comment.ResolvedAt, commentSaved.ResolvedAt);
        Assert.AreEqual(resolvedBy.Email, user.Email);
        Assert.AreEqual(resolvedBy.Name, user.Name);
        Assert.AreEqual(resolvedBy.LastName, user.LastName);
        Assert.AreEqual(resolvedBy.BirthDate, user.BirthDate);
    }
    
    [TestMethod]
    public void DeleteCommentRepository()
    {
        //Arrange 
        User resolvedBy = new User()
        {
            Name = "Name",
            LastName = "LastName",
            Email = "email@email.com",
            BirthDate = new DateTime(2008, 6, 1, 7, 47, 0),
            Password = "password",
            IsAdmin = false,
            Trash = new Trash()
        };
        Comment comment = new Comment()
        {
            Id = 123,
            Message = "Message comment",
            ResolvedBy = resolvedBy,
            ResolvedAt = new DateTime(2008, 6, 1, 7, 47, 0),
            Status = EStatusComment.RESOLVED
        };
        _commentRepository.Add(comment);
        
        //Act 
        Comment commentDelete = _commentRepository.Delete(comment.Id);

        // Assert
        Assert.AreEqual(comment.Id, commentDelete.Id);
        Assert.AreEqual(comment.Message, commentDelete.Message);
        Assert.AreEqual(0, _commentRepository.GetAll().Count);
    }
    
    [TestMethod]
    public void NotExistCommentDeleteCommentRepository()
    {
        //Arrange 
        User resolvedBy = new User()
        {
            Name = "Name",
            LastName = "LastName",
            Email = "email@email.com",
            BirthDate = new DateTime(2008, 6, 1, 7, 47, 0),
            Password = "password",
            IsAdmin = false,
            Trash = new Trash()
        };
        Comment comment = new Comment()
        {
            Id = 123,
            Message = "Message comment",
            ResolvedBy = resolvedBy,
            ResolvedAt = new DateTime(2008, 6, 1, 7, 47, 0),
            Status = EStatusComment.RESOLVED
        };
        
        //Act 
        var exception = Assert.ThrowsException<CommentNotValidException>(() => _commentRepository.Delete(comment.Id));

        // Assert
        Assert.AreEqual($"Comment with id: {comment.Id} does not exist", exception.Message);
    }
    
    [TestMethod]
    public void GetAllCommentRepository()
    {
        //Arrange 
        User resolvedBy = new User();
        Comment comment = new Comment()
        {
            Id = 123,
            Message = "Message comment",
            ResolvedBy = resolvedBy,
            ResolvedAt = new DateTime(2008, 6, 1, 7, 47, 0),
            Status = EStatusComment.RESOLVED
        };
        
        //Act 
        _commentRepository.Add(comment);
        List<Comment> comments = _commentRepository.GetAll();

        // Assert
        Assert.AreEqual(1, comments.Count);
    }
    
    [TestMethod]
    public void UpdateCommentRepository()
    {
        //Arrange 
        User resolvedBy = new User();
        Comment comment = new Comment()
        {
            Id = 123,
            Message = "Message comment",
            ResolvedBy = resolvedBy,
            ResolvedAt = new DateTime(2008, 6, 1, 7, 47, 0),
            Status = EStatusComment.PENDING
        };
        
        //Act 
        _commentRepository.Add(comment);

        comment.Message = "New message";
        comment.Status = EStatusComment.RESOLVED;
        Comment commentUpdated = _commentRepository.Update(comment);
        

        // Assert
        Assert.AreEqual("New message", commentUpdated.Message);
        Assert.AreEqual(EStatusComment.RESOLVED, commentUpdated.Status);
    }
    
    
    [TestCleanup]
    public void Cleanup()
    {
        _commentRepository.GetAll().Clear();
    }
    
}