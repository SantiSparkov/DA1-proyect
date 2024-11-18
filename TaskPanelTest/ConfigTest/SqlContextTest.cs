using Microsoft.EntityFrameworkCore;
using TaskPanelLibrary.Config;
using TaskPanelLibrary.Entity;
using Task = System.Threading.Tasks.Task;

namespace TaskPanelTest.ConfigTest;

public class SqlContexTest
{

    public SqlContext CreateMemoryContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<SqlContext>();
        optionsBuilder.UseInMemoryDatabase("DataBaseInMemory");
        return new SqlContext(optionsBuilder.Options);
    }
    
    public DbSet<User> Users { get; set; }
    
    public DbSet<Team> Teams { get; set; }
    
    public DbSet<Trash> Trashes { get; set; }
    
    public DbSet<Task> Tasks { get; set; }
    
    public DbSet<Panel> Panels { get; set; }
    
    public DbSet<Comment> Comments { get; set; }
    
    public DbSet<Epic> Epics { get; set; }

    public DbSet<Notification> Notifications { get; set; }
}