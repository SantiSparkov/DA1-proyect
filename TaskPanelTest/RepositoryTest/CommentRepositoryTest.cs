using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Entity.Enum;
using TaskPanelLibrary.Exception.Comment;
using TaskPanelLibrary.Repository;
using TaskPanelLibrary.Repository.Interface;

namespace TaskPanelTest.RepositoryTest;

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
            TrashId = 1
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
        _commentRepository.AddComment(comment);
        Comment commentSaved = _commentRepository.GetCommentById(comment.Id);
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
            TrashId = 1
        };
        Comment comment = new Comment()
        {
            Id = 123,
            Message = "Message comment",
            ResolvedBy = resolvedBy,
            ResolvedAt = new DateTime(2008, 6, 1, 7, 47, 0),
            Status = EStatusComment.RESOLVED
        };
        _commentRepository.AddComment(comment);
        
        //Act 
        Comment commentDelete = _commentRepository.DeleteComment(comment.Id);

        // Assert
        Assert.AreEqual(comment.Id, commentDelete.Id);
        Assert.AreEqual(comment.Message, commentDelete.Message);
        Assert.AreEqual(0, _commentRepository.GetAllComments().Count);
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
            TrashId = 1
        };
        Comment comment = new Comment()
        {
            Id = 123,
            Message = "Message comment",
            ResolvedBy = resolvedBy,
            ResolvedAt = new DateTime(2008, 6, 1, 7, 47, 0),
            Status = EStatusComment.RESOLVED
        };
        
        Comment commentNotExist = new Comment()
        {
            Id = 321,
            Message = "Message comment",
            ResolvedBy = resolvedBy,
            ResolvedAt = new DateTime(2008, 6, 1, 7, 47, 0),
            Status = EStatusComment.RESOLVED
        };
        
        _commentRepository.AddComment(comment);
        
        //Act 
        var exception = Assert.ThrowsException<CommentNotValidException>(() => _commentRepository.DeleteComment(commentNotExist.Id));

        // Assert
        Assert.AreEqual($"Comment with id: {commentNotExist.Id} does not exist", exception.Message);
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
        
        Comment comment2= new Comment()
        {
            Id = 321,
            Message = "Message comment",
            ResolvedBy = resolvedBy,
            ResolvedAt = new DateTime(2008, 6, 1, 7, 47, 0),
            Status = EStatusComment.RESOLVED
        };
        
        _commentRepository.AddComment(comment2);
        
        //Act 
        _commentRepository.AddComment(comment);
        List<Comment> comments = _commentRepository.GetAllComments();

        // Assert
        Assert.AreEqual(2, comments.Count);
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
        _commentRepository.AddComment(comment);

        comment.Message = "New message";
        comment.Status = EStatusComment.RESOLVED;
        Comment commentUpdated = _commentRepository.UpdateComment(comment);
        

        // Assert
        Assert.AreEqual("New message", commentUpdated.Message);
        Assert.AreEqual(EStatusComment.RESOLVED, commentUpdated.Status);
    }
    
    
    [TestMethod]
    public void GetCommentByIdNotExistRepository()
    {
        //Arrange
        Comment comment = new Comment()
        {
            Id = 123,
            Message = "Message comment",
            ResolvedAt = new DateTime(2008, 6, 1, 7, 47, 0),
            Status = EStatusComment.PENDING
        };
        
        Comment comment2 = new Comment()
        {
            Id = 12,
            Message = "Message comment",
            ResolvedAt = new DateTime(2008, 6, 1, 7, 47, 0),
            Status = EStatusComment.PENDING
        };
        _commentRepository.AddComment(comment2);

        //Act 
        var exception = Assert.ThrowsException<CommentNotValidException>(() => _commentRepository.GetCommentById(comment.Id));

        // Assert
        Assert.AreEqual($"Panel with id: {comment.Id} does not exist", exception.Message);
    }
    
    
    [TestCleanup]
    public void Cleanup()
    {
        _commentRepository.GetAllComments().Clear();
    }
    
}