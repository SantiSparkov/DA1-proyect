using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Entity.Enum;

namespace TaskPanelTest.EntityTest;

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
            Status = EStatusComment.RESOLVED,
            TaskId = 1,
            CreatedBy = new User(),
            CreatedById = 1
        };

        // ASSERT

        Assert.AreEqual(1, comment.Id);
        Assert.IsNotNull(comment.ResolvedBy);
        Assert.AreEqual("Message", comment.Message);
        Assert.AreEqual(new DateTime(2008, 6, 1, 7, 47, 0), comment.ResolvedAt);
        Assert.AreEqual(EStatusComment.RESOLVED, comment.Status);
        Assert.IsNotNull(comment.CreatedBy);
        Assert.AreEqual(1, comment.CreatedById);
        Assert.AreEqual(1, comment.TaskId);
    }
}