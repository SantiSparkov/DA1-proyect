using Moq;
   using TaskPanelLibrary.Entity;
   using TaskPanelLibrary.Entity.Enum;
   using TaskPanelLibrary.Exception.Comment;
   using TaskPanelLibrary.Repository.Interface;
   using TaskPanelLibrary.Service;
   using Microsoft.VisualStudio.TestTools.UnitTesting;
   
   namespace TaskPanelTest.ServiceTest
   {
       [TestClass]
       public class CommentServiceTest
       {
           private Mock<ICommentRepository> _mockCommentRepository;
           private CommentService _commentService;
           private List<Comment> _mockComments;
           private User _user;
   
           [TestInitialize]
           public void Setup()
           {
               _mockCommentRepository = new Mock<ICommentRepository>();
               
               _user = new User { Id = 1, Name = "John", LastName = "Smith" ,Email = "a@gmail.com", Password = "123", IsAdmin = true };
               
               _mockComments = new List<Comment>
               {
                   new Comment { Id = 1, Message = "Test comment 1", Status = EStatusComment.PENDING, TaskId = 101 },
                   new Comment { Id = 2, Message = "Test comment 2", Status = EStatusComment.RESOLVED, TaskId = 102, ResolvedBy = _user, ResolvedAt = DateTime.Now },
               };
   
               _mockCommentRepository.Setup(repo => repo.GetAllComments()).Returns(_mockComments);
               _commentService = new CommentService(_mockCommentRepository.Object);
           }
   
           [TestMethod]
           public void CreateComment_ValidComment_ShouldAddComment()
           {
               // Arrange
               var newComment = new Comment
               {
                   Id = 3,
                   Message = "New comment",
                   Status = EStatusComment.PENDING,
                   TaskId = 103
               };
   
               _mockCommentRepository.Setup(repo => repo.AddComment(It.IsAny<Comment>())).Verifiable();
   
               // Act
               var createdComment = _commentService.CreateComment(newComment);
   
               // Assert
               Assert.AreEqual(newComment.Message, createdComment.Message);
               Assert.AreEqual(newComment.Status, createdComment.Status);
               Assert.AreEqual(newComment.TaskId, createdComment.TaskId);
               _mockCommentRepository.Verify(repo => repo.AddComment(It.IsAny<Comment>()), Times.Once);
           }
   
           [TestMethod]
           [ExpectedException(typeof(CommentNotValidException))]
           public void CreateComment_NullComment_ShouldThrowException()
           {
               // Act
               var result = _commentService.CreateComment(null);
               
                // Assert
                Assert.IsNull(result);
           }
   
           [TestMethod]
           [ExpectedException(typeof(CommentNotValidException))]
           public void CreateComment_EmptyMessage_ShouldThrowException()
           {
               // Arrange
               var invalidComment = new Comment
               {
                   Id = 3,
                   Message = "",
                   Status = EStatusComment.PENDING,
                   TaskId = 103
               };
   
               // Act
               _commentService.CreateComment(invalidComment);
           }
   
           [TestMethod]
           [ExpectedException(typeof(CommentNotValidException))]
           public void CreateComment_ResolvedWithoutResolvedBy_ShouldThrowException()
           {
               // Arrange
               var invalidComment = new Comment
               {
                   Id = 3,
                   Message = "Resolved comment",
                   Status = EStatusComment.RESOLVED,
                   TaskId = 103
               };
   
               // Act
               _commentService.CreateComment(invalidComment);
           }
   
           [TestMethod]
           public void GetCommentById_ValidId_ShouldReturnComment()
           {
               // Act
               var comment = _commentService.GetCommentById(1);
   
               // Assert
               Assert.AreEqual(1, comment.Id);
               Assert.AreEqual("Test comment 1", comment.Message);
           }
   
           [TestMethod]
           [ExpectedException(typeof(CommentNotValidException))]
           public void GetCommentById_InvalidId_ShouldThrowException()
           {
               // Act
               _commentService.GetCommentById(99);
           }
   
           [TestMethod]
           public void UpdateComment_ValidComment_ShouldUpdateAndReturnComment()
           {
               // Arrange
               var updatedComment = new Comment
               {
                   Id = 1,
                   Message = "Updated comment",
                   Status = EStatusComment.RESOLVED,
                   ResolvedBy = _user,
                   ResolvedAt = DateTime.Now
               };
   
               _mockCommentRepository.Setup(repo => repo.UpdateComment(It.IsAny<Comment>())).Verifiable();
   
               // Act
               var result = _commentService.UpdateComment(updatedComment);
   
               // Assert
               Assert.AreEqual(updatedComment.Message, result.Message);
               Assert.AreEqual(updatedComment.Status, result.Status);
               _mockCommentRepository.Verify(repo => repo.UpdateComment(It.IsAny<Comment>()), Times.Once);
           }
   
           [TestMethod]
           public void GetCommentForTask_ValidTaskId_ShouldReturnComments()
           {
               // Act
               var comments = _commentService.GetCommentForTask(101);
   
               // Assert
               Assert.AreEqual(1, comments.Count);
               Assert.AreEqual("Test comment 1", comments[0].Message);
           }
   
           [TestMethod]
           public void GetCommentForTask_InvalidTaskId_ShouldReturnEmptyList()
           {
               // Act
               var comments = _commentService.GetCommentForTask(999);
   
               // Assert
               Assert.AreEqual(0, comments.Count);
           }
       }
   }
   