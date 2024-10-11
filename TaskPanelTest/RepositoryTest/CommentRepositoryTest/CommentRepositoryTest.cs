using TaskPanelLibrary.Entity;
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
            Status = Comment.EStatus.RESOLVED
        };
        
        //Act 
        _commentRepository.add(comment);
        Comment commentSaved = _commentRepository.finById(comment.Id);
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
            Status = Comment.EStatus.RESOLVED
        };
        _commentRepository.add(comment);
        
        //Act 
        Comment commentDelete = _commentRepository.delete(comment.Id);

        // Assert
        Assert.AreEqual(comment.Id, commentDelete.Id);
        Assert.AreEqual(comment.Message, commentDelete.Message);
        Assert.AreEqual(0, _commentRepository.getAll().Count);
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
            Status = Comment.EStatus.RESOLVED
        };
        
        //Act 
        var exception = Assert.ThrowsException<System.ArgumentException>(() => _commentRepository.delete(comment.Id));


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
            Status = Comment.EStatus.RESOLVED
        };
        
        //Act 
        _commentRepository.add(comment);
        List<Comment> comments = _commentRepository.getAll();

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
            Status = Comment.EStatus.PENDING
        };
        
        //Act 
        _commentRepository.add(comment);

        comment.Message = "New message";
        comment.Status = Comment.EStatus.RESOLVED;
        Comment commentUpdated = _commentRepository.update(comment);
        

        // Assert
        Assert.AreEqual("New message", commentUpdated.Message);
        Assert.AreEqual(Comment.EStatus.RESOLVED, commentUpdated.Status);
    }
    
    
    [TestCleanup]
    public void Cleanup()
    {
        _commentRepository.getAll().Clear();
    }
    
}