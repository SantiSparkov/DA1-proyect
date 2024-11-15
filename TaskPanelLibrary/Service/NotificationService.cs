using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Entity.Enum;
using TaskPanelLibrary.Exception;
using TaskPanelLibrary.Exception.Comment;
using TaskPanelLibrary.Repository;
using TaskPanelLibrary.Repository.Interface;
using TaskPanelLibrary.Service.Interface;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelLibrary.Service;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;

    private readonly AuthService _authService;
    
    public Action OnNotificationAdded { get; set; }

    public NotificationService(INotificationRepository notificationRepository, AuthService authService)
    {
        _notificationRepository = notificationRepository;
        _authService = authService;
    }

    public Notification CreateNotification(int id, string message)
    {
        if (message == "")
            throw new NotificationNotValidException("Notification can not be empty.");

        Notification notification = new Notification()
        {
            Message = "Comment #" + id + " has been resolved. Message: " + message,
            User = _authService.GetCurrentUser(),
            UserId = _authService.GetCurrentUser().Id
            
        };
        _notificationRepository.CreateNotification(notification);
        OnNotificationAdded?.Invoke();
        return notification;
    }

    public List<Notification> GetNotifications(int id)
    {
        return _notificationRepository.GetNotifications(id);
    }

    public void DeleteNotification(int id)
    {
        _notificationRepository.DeleteNotification(id);
        OnNotificationAdded?.Invoke();
    }
}