using Microsoft.EntityFrameworkCore;
using TaskPanelLibrary.Config;
using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Entity.Enum;
using TaskPanelLibrary.Exception.Comment;
using TaskPanelLibrary.Repository;
using TaskPanelLibrary.Repository.Interface;
using TaskPanelTest.ConfigTest;

namespace TaskPanelTest.RepositoryTest.SqlRepositories;

[TestClass]
public class CommentSqlRepositoryTest
{

    private SqlContext _sqlContext;

    private ICommentRepository _commentSqlRepository;
    
    [TestInitialize]
    public void Initialize()
    {
        _sqlContext = new SqlContexTest().CreateMemoryContext();
        _commentSqlRepository = new CommentSqlRepository(_sqlContext);
    }
    
    [TestCleanup]
    public void CleanUp()
    {
        _sqlContext?.Database.EnsureDeleted();
    }

    [TestMethod]
    public void AddCommentSql()
    {
        // Arrange
        Comment comment = new Comment()
        {
            Id = 1,
            CreatedById = 1,
            Message = "Comment test",
            Status = EStatusComment.PENDING
        };

        // Act
        _commentSqlRepository.AddComment(comment);

        // Assert
        Assert.AreEqual(1, _commentSqlRepository.GetAllComments().Count);

    }
    
    [TestMethod]
    public void GetCommentById()
    {
        // Arrange
        Comment comment = new Comment()
        {
            Id = 1,
            CreatedById = 1,
            Message = "Comment test",
            Status = EStatusComment.PENDING
        };

        // Act
        _commentSqlRepository.AddComment(comment);

        Comment commentSaved = _commentSqlRepository.GetCommentById(1);

        // Assert
        Assert.AreEqual(1, commentSaved.Id);
        Assert.AreEqual(1, commentSaved.CreatedById);
        Assert.AreEqual("Comment test", commentSaved.Message);
        Assert.AreEqual(EStatusComment.PENDING, commentSaved.Status);
    }
    
    [TestMethod]
    public void GetCommentByIdNotExist()
    {
        // Arrange
        Comment comment = new Comment()
        {
            Id = 1,
            CreatedById = 1,
            Message = "Comment test",
            Status = EStatusComment.PENDING
        };

        //Act 
        var exception = Assert.ThrowsException<CommentNotValidException>(() => _commentSqlRepository.GetCommentById(comment.Id));

        // Assert
        Assert.AreEqual($"Comment with id: {comment.Id} does not exist", exception.Message);
    }
    
    [TestMethod]
    public void DeleteComment()
    {
        // Arrange
        Comment comment = new Comment()
        {
            Id = 1,
            CreatedById = 1,
            Message = "Comment test",
            Status = EStatusComment.PENDING
        };

        // Act
        _commentSqlRepository.AddComment(comment);

        Comment commentDeleted = _commentSqlRepository.DeleteComment(comment.Id);

        // Assert
        Assert.AreEqual(comment.Id, commentDeleted.Id);
        Assert.AreEqual(comment.CreatedById, commentDeleted.CreatedById);
        Assert.AreEqual(comment.Message, commentDeleted.Message);
        Assert.AreEqual(comment.Status, commentDeleted.Status);
    }
    
    [TestMethod]
    public void DeleteCommentNotExist()
    {
        // Arrange
        Comment comment = new Comment()
        {
            Id = 1,
            CreatedById = 1,
            Message = "Comment test",
            Status = EStatusComment.PENDING
        };

        //Act 
        var exception = Assert.ThrowsException<CommentNotValidException>(() => _commentSqlRepository.DeleteComment(comment.Id));

        // Assert
        Assert.AreEqual($"Comment with id: {comment.Id} does not exist", exception.Message);
    }
    
    [TestMethod]
    public void UpdateComment()
    {
        // Arrange
        Comment comment = new Comment()
        {
            Id = 1,
            CreatedById = 1,
            Message = "Comment test",
            Status = EStatusComment.PENDING
        };

        // Act
        _commentSqlRepository.AddComment(comment);

        comment.Message = "Update comment";
        comment.Status = EStatusComment.RESOLVED;

        Comment commentUpdated = _commentSqlRepository.UpdateComment(comment);

        // Assert
        Assert.AreEqual("Update comment", commentUpdated.Message);
        Assert.AreEqual(EStatusComment.RESOLVED, commentUpdated.Status);
    }
    
}