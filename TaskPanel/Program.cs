using TaskPanelLibrary.DataTest;
using TaskPanelLibrary.Repository;
using TaskPanelLibrary.Repository.Interface;
using TaskPanelLibrary.Service;
using TaskPanelLibrary.Service.Interface;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
    
// Repositories

builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<ITeamRepository, TeamRepository>();
builder.Services.AddSingleton<IPanelRepository, PanelRepository>();
builder.Services.AddSingleton<ITaskRepository, TaskRepository>();
builder.Services.AddSingleton<ICommentRepository, CommentRepository>();

// Services
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<ITeamService, TeamService>();
builder.Services.AddSingleton<IPanelService, PanelService>();
builder.Services.AddSingleton<ITaskService, TaskService>();
builder.Services.AddSingleton<ICommentService, CommentService>();
builder.Services.AddSingleton<PasswordGeneratorService>();
builder.Services.AddSingleton<AuthService>();
builder.Services.AddSingleton<ImportCsvService>();

//Data for test
builder.Services.AddSingleton<Panels>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();