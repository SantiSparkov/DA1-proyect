using Microsoft.AspNetCore.Components.Forms;
using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Entity.Enum;
using TaskPanelLibrary.Service.Interface;
using Task = System.Threading.Tasks.Task;

namespace TaskPanelLibrary.Service
{
    public class ImportCsvService : IImportService
    {
        private readonly ITaskService _taskService;
        
        private readonly IPanelService _panelService;
        
        private readonly IEpicService _epicService;

        public ImportCsvService(ITaskService taskService, IPanelService panelService, IEpicService epicService)
        {
            _taskService = taskService;
            _panelService = panelService;
            _epicService = epicService;
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

                if (columns.Length != 7)
                {
                    LogError(logFile, line, "Invalid format: 7 columns required");
                    continue;
                }

                var title = columns[0];
                var description = columns[1];
                if (!DateTime.TryParse(columns[2], out var dueDate))
                {
                    LogError(logFile, line, "Invalid date format");
                    continue;
                }

                if (!int.TryParse(columns[3], out var panelId))
                {
                    LogError(logFile, line, "Panel ID not found");
                    continue;
                }

                try
                {
                    _panelService.GetPanelById(panelId);
                }
                catch (System.Exception ex)
                {
                    LogError(logFile, line, $"Panel ID not found: {ex.Message}");
                    continue;
                }

                if (!Enum.TryParse<EPriority>(columns[4], true, out var priority))
                {
                    LogError(logFile, line, "Invalid priority value");
                    continue;
                }

                if (!int.TryParse(columns[5], out var epicId))
                {
                    LogError(logFile, line, "Invalid Epic ID");
                    continue;
                }
                
                try
                {
                    _epicService.GetEpicById(epicId);
                }
                catch (System.Exception ex)
                {
                    LogError(logFile, line, "Epic does not exist.");
                    continue;
                }

                if (!int.TryParse(columns[6], out var estimatedEffortHours))
                {
                    LogError(logFile, line, "Invalid Estimated Effort Hours");
                    continue;
                }

                var task = new Entity.Task
                {
                    Title = title,
                    Description = description,
                    DueDate = dueDate,
                    PanelId = panelId,
                    Priority = priority,
                    EpicId = epicId,
                    InvertedEstimateHour = estimatedEffortHours
                };
                
                try
                {
                    _taskService.CreateTask(task);
                }
                catch (System.Exception ex)
                {
                    LogError(logFile, line, $"Failed to create task: {ex.Message}");
                }
            }

            await logFile.FlushAsync();
        }

        private void LogError(StreamWriter logFile, string line, string errorMessage)
        {
            var timestamp = DateTime.UtcNow.ToString("o");
            var logLine = $"{timestamp} - Error: {errorMessage} - Line: {line}";
            logFile.WriteLine(logLine);
        }
    }
}