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
            
            Team newTeam = new Team()
            {
                CreationDate = DateTime.Now, Name = "Team name", TasksDescription = "Description team", MaxAmountOfMembers = 20
            };
            Team team = _teamService.CreateTeam(newTeam, user.Id);

            Panel newPanel = new Panel()
            {
                Name = "Panel test",
                Description = "Task",
                Team = team
            };
            _panelService.CreatePanel(newPanel, user.Id);

            Task newtask = new Task()
            {
                Title = "Title task",
                Description = "description task",
                PanelId = newPanel.Id,
                Priority = ETaskPriority.LOW
            };
            _taskService.CreateTask(newtask);

            Comment newComment = new Comment()
            {
                Message = "Comment test", 
                Status = EStatusComment.PENDING, 
                TaskId = newtask.Id,
                CreatedBy = _authService.GetCurrentUser(),
                CreatedById = _authService.GetCurrentUser().Id
            };
            _commentService.CreateComment(newComment);
        }
    }
}

