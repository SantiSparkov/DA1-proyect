using TaskPanelLibrary.Entity;

namespace TaskPanelLibrary.Repository.Interface;

public interface INotificationRepository
{
    Notification CreateNotification(Notification notification);
    
    List<Notification> GetNotifications(int id);
    
    void DeleteNotification(int id);
}