using Microsoft.EntityFrameworkCore;
using TaskPanelLibrary.Entity;

namespace TaskPanelLibrary.Config;

public class SqlContext : DbContext
{
    public SqlContext(DbContextOptions<SqlContext> options) : base(options)
    {
        this.Database.Migrate();
    }
    
}