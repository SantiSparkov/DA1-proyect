using TaskPanelLibrary.Config;
using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Repository.Interface;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelLibrary.Repository;

public class NotificationSqlRepository : INotificationRepository
{
    private SqlContext _notificationDatabase;

    public NotificationSqlRepository(SqlContext sqlContext)
    {
        _notificationDatabase = sqlContext;
    }

    public Notification CreateNotification(Notification notification)
    {
        _notificationDatabase.Notifications.Add(notification);
        _notificationDatabase.SaveChanges();
        
        return notification;
    }

    public List<Notification> GetNotifications(int id)
    {
        return _notificationDatabase.Notifications.Where(n => n.UserId == id).ToList();
    }

    public void DeleteNotification(int id)
    {
        Notification notification = _notificationDatabase.Notifications.Find(id);
        _notificationDatabase.Notifications.Remove(notification);
        _notificationDatabase.SaveChanges();
    }
    
}