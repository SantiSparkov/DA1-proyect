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
            User = "User",
            Message = "Message",
            CreateDate = new DateTime(2008, 6, 1, 7, 47, 0),
            Status = EStatus.Active
        };
        
        // ASSERT
        
        Assert.AreEqual("User", comment.User);
        Assert.AreEqual("Message", comment.Message);
        Assert.AreEqual(new DateTime(2008, 6, 1, 7, 47, 0), comment.CreateDate);
        Assert.AreEqual(EStatus.Active, comment.Status);
    }
}