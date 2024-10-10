using TaskPanelLibrary.Entity;

namespace TaskPanelTest.EntityTest.CommentEntityTest;


[TestClass]
public class CommentEntityTest
{
    [TestMethod]
    public void TestCommentEntity()
    {
        Comment comment = new Comment()
        {
            Id = 1,
            ResolvedBy = new User(),
            Message = "Message",
            ResolvedAt = new DateTime(2008, 6, 1, 7, 47, 0),
            Status = Comment.EStatus.RESOLVED
        };
        
        // ASSERT

        Assert.AreEqual(1, comment.Id);
        Assert.IsNotNull(comment.ResolvedBy);
        Assert.AreEqual("Message", comment.Message);
        Assert.AreEqual(new DateTime(2008, 6, 1, 7, 47, 0), comment.ResolvedAt);
        Assert.AreEqual(Comment.EStatus.RESOLVED, comment.Status);
    }
}