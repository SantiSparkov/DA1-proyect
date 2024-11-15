using Microsoft.EntityFrameworkCore;
using TaskPanelLibrary.Entity;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelLibrary.Config;

public class SqlContext : DbContext
{
    public SqlContext(DbContextOptions<SqlContext> options) : base(options)
    {
        this.Database.Migrate();
    }

    public DbSet<User> Users { get; set; }
    
    public DbSet<Team> Teams { get; set; }
    
    public DbSet<Trash> Trashes { get; set; }
    
    public DbSet<Task> Tasks { get; set; }
    
    public DbSet<Panel> Panels { get; set; }
    
    public DbSet<Comment> Comments { get; set; }
    
    public DbSet<Notification> Notifications { get; set; }

}