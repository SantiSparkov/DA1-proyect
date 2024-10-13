using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Entity.Enum;
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
    
    public Panels(IUserService userService, IPanelService panelService, ITeamService teamService, ITaskService taskService, ICommentService commentService)
    {
        _userService = userService;
        _panelService = panelService;
        _teamService = teamService;
        _taskService = taskService;
        _commentService = commentService;
    }

    public void createTeamAndPanel()
    {
        if (_userService.GetAllUsers().Count == 0)
        {

            User user = new User()
            {
                Id = 1, Email = "test@mail.com", Name = "Fran", LastName = "Sosa", Trash = new Trash(), IsAdmin = true,
                Password = "password"
            };
            _userService.AddUser(user);
            Team team = _teamService.CreateTeam(new Team()
            {
                Panels = new List<Panel>(), Id = 1, Name = "Name panel", TasksDescription = "description",
                TeamLeader = user
            }, user.Id);
            Panel panel = _panelService.CreatePanel(user);
            Task task = new Task()
            {
                Id = 1,
                Description = "desc task 1", Priority = EStatusComment.PENDING,
                Title = "Title task 1", PanelId = panel.Id
            };
            Comment comment = _commentService.CreateComment();
            comment.TaskId = task.Id;
            _taskService.AddTask(task);
            _taskService.AddComentToTask(task.Id, comment);
            _panelService.AddTask(panel.Id, task);
            
            Panel panel2 = _panelService.CreatePanel(user);
            panel2.Name = "Panel 2";
        }
    }
}

