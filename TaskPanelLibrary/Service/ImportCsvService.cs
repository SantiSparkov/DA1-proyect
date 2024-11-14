using Microsoft.AspNetCore.Components.Forms;
using TaskPanelLibrary.Entity.Enum;
using TaskPanelLibrary.Service.Interface;

namespace TaskPanelLibrary.Service;

public class ImportCsvService
{
    private readonly ITaskService _taskService;

    private readonly IPanelService _panelService;

    public ImportCsvService(ITaskService taskService, IPanelService panelService)
    {
        _taskService = taskService;
        _panelService = panelService;
    }

    public async Task ImportTasksFromFile(IBrowserFile file, string userName)
    {
        using var stream = file.OpenReadStream();
        using var reader = new StreamReader(stream);

        var logFilePath = $"ErroresImport-{userName}.txt";
        using var logFile = new StreamWriter(logFilePath, append: true);

        string line;

        while ((line = await reader.ReadLineAsync()) != null)
        {
            var columns = line.Split(',');

            if (columns.Length != 5)
            {
                LogError(logFile, line, "Invalid format: 5 columns required", userName);
                continue;
            }

            var title = columns[0];
            var description = columns[1];
            if (!DateTime.TryParse(columns[2], out var dueDate))
            {
                LogError(logFile, line, "Invalid date format", userName);
                continue;
            }

            if (!int.TryParse(columns[3], out var panelId))
            {
                LogError(logFile, line, "Panel ID not found", userName);
                continue;
            }

            try
            {
                _panelService.GetPanelById(panelId);
            }
            catch (System.Exception ex)
            {
                LogError(logFile, line, $"Panel ID not found: {ex.Message}", userName);
                continue;
            }

            if (!Enum.TryParse<EPriority>(columns[4], true, out var priority))
            {
                LogError(logFile, line, "Invalid priority value", userName);
                continue;
            }

            var task = new Entity.Task
            {
                Title = title,
                Description = description,
                DueDate = dueDate,
                PanelId = panelId,
                Priority = priority
            };
            
            _taskService.CreateTask(task);
        }

        await logFile.FlushAsync();
    }


    private void LogError(StreamWriter logFile, string line, string errorMessage, string userName)
    {
        var timestamp = DateTime.UtcNow.ToString("o");
        var logLine = $"{timestamp} - Error: {errorMessage} - Line: {line}";
        logFile.WriteLine(logLine);
    }
}