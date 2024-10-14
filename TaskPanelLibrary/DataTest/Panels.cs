using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Entity.Enum;
using TaskPanelLibrary.Repository;
using TaskPanelLibrary.Repository.Interface;
using TaskPanelLibrary.Service;
using TaskPanelLibrary.Service.Interface;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelLibrary.DataTest;

public class Panels
{
    private IUserService _userService;

    private IPanelService _panelService;

    private ITeamService _teamService;

    private ITaskService _taskService;

    private ICommentService _commentService;

    private AuthService _authService;
    
    public Panels(IUserService userService, IPanelService panelService, ITeamService teamService, ITaskService taskService, ICommentService commentService, AuthService authService)
    {
        _userService = userService;
        _panelService = panelService;
        _teamService = teamService;
        _taskService = taskService;
        _commentService = commentService;
        _authService = authService;
    }

    public void createTeamAndPanel()
    {
        if (_panelService.GetAllPanels().Count == 0)
        {

            User user = _authService.GetCurrentUser();
            
            Team team = _teamService.CreateTeam(new Team()
            {
                Panels = new List<Panel>(), Id = 1, Name = "Name panel", TasksDescription = "description",
                TeamLeader = user
            }, user.Id);
            Panel panel = _panelService.CreatePanel(user);
            Task task = new Task()
            {
                Id = 1,
                Description = "desc task 1", Priority = ETaskPriority.LOW,
                Title = "Title task 1", PanelId = panel.Id
            };
            Comment comment = new Comment();
            comment.TaskId = task.Id;
            comment.Message = "Comentario de prueba!";
            comment.Status = EStatusComment.PENDING;
            comment.ResolvedBy = user;
            comment.ResolvedAt = DateTime.Now;
            _commentService.CreateComment(comment);
            _taskService.AddTask(task);
            _taskService.AddComentToTask(task.Id, comment);
            _panelService.AddTask(panel.Id, task);
            
            Panel panel2 = _panelService.CreatePanel(user);
            panel2.Name = "Panel 2";
        }
    }
}

