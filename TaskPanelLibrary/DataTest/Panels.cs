using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Service;
using TaskPanelLibrary.Service.Interface;

namespace TaskPanelLibrary.DataTest;

public class Panels
{
    private IUserService _userService;

    private IPanelService _panelService;

    private ITeamService _teamService;

    public Panels(IUserService userService, IPanelService panelService, ITeamService teamService)
    {
        _userService = userService;
        _panelService = panelService;
        _teamService = teamService;
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
            
            Panel panel2 = _panelService.CreatePanel(user);
        }
    }
}

