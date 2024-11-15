using System.ComponentModel.DataAnnotations;
using TaskPanelLibrary.Entity.Enum;

namespace TaskPanelLibrary.Entity;

public class Notification
{
    public int Id { get; set; }
    
    public String Message { get; set; }
    
    public int UserId { get; set; }
    
    public User User { get; set; }
}