using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Exception;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelTest.EntityTest;

[TestClass]
public class TrashEntityTest
{
    private Trash trash;
    
    [TestInitialize]
    public void Initialize()
    {
        trash = new Trash();
    }
}