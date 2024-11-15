using TaskPanelLibrary.Entity;

namespace TaskPanelLibrary.Service.Interface;

public interface INotificationService
{
    Action OnNotificationAdded { get; set; }
    
    Notification CreateNotification(int id, string message);
    
    List<Notification> GetNotifications(int id);
    
    void DeleteNotification(int notificationId);
}