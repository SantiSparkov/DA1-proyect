using Microsoft.EntityFrameworkCore;
using TaskPanelLibrary.Config;
using TaskPanelLibrary.DataTest;
using TaskPanelLibrary.Repository;
using TaskPanelLibrary.Repository.Interface;
using TaskPanelLibrary.Service;
using TaskPanelLibrary.Service.Interface;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
    
// Repositories 
builder.Services.AddScoped<IUserRepository, UserSqlRepository>();
builder.Services.AddScoped<ITeamRepository, TeamSqlRepository>();
builder.Services.AddScoped<IPanelRepository, PanelSqlRepository>();
builder.Services.AddScoped<ITaskRepository, TaskSqlRepository>();
builder.Services.AddScoped<ICommentRepository, CommentSqlRepository>();
builder.Services.AddScoped<ITrashRepository, TrashSqlRepository>();

// Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IPanelService, PanelService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<PasswordGeneratorService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ImportCsvService>();
builder.Services.AddScoped<ITrashService, TrashService>();


//Data for test
builder.Services.AddScoped<Panels>();

// Database configuration
builder.Services.AddDbContextFactory<SqlContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        providerOptions => providerOptions.EnableRetryOnFailure()
        )
);
    
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