namespace TaskPanelTest.EntityTest.CommentEntityTest;
using TaskPanel.Models.Entity;
using EStatus = TaskPanel.Models.Entity.Comment.EStatus;

[TestClass]
public class CommentEntityTest
{
    [TestMethod]
    public void TestCommentEntity()
    {
        Comment comment = new Comment()
        {
            CommentId = 1,
            ResolvedBy = new User(),
            Message = "Message",
            ResolvedAt = new DateTime(2008, 6, 1, 7, 47, 0),
            Status = EStatus.RESOLVED
        };
        
        // ASSERT
        
        Assert.AreEqual(1, comment.CommentId);
        Assert.AreEqual("User", comment.ResolvedBy);
        Assert.AreEqual("Message", comment.Message);
        Assert.AreEqual(new DateTime(2008, 6, 1, 7, 47, 0), comment.ResolvedAt);
        Assert.AreEqual(EStatus.RESOLVED, comment.Status);
    }
}