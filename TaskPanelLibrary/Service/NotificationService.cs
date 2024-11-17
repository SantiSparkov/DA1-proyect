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

    private readonly IUserService _userService;
    
    public Action OnNotificationAdded { get; set; }

    public NotificationService(INotificationRepository notificationRepository, IUserService userService)
    {
        _notificationRepository = notificationRepository;
        _userService = userService;
    }

    public Notification CreateNotification(int userId, string message)
    {
        if (message == "")
            throw new NotificationNotValidException("Notification can not be empty.");

        Notification notification = new Notification()
        {
            Message = "Comment has been resolved. Message: " + message,
            User = _userService.GetUserById(userId),
            UserId = userId
            
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