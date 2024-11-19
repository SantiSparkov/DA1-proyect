using Microsoft.AspNetCore.Components.Forms;
using Microsoft.VisualBasic.CompilerServices;
using OfficeOpenXml;
using TaskPanelLibrary.Entity.Enum;
using TaskPanelLibrary.Service.Interface;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace TaskPanelLibrary.Service
{
    public class ImportXlsxService : IImportService
    {
        private readonly ITaskService _taskService;
        private readonly IPanelService _panelService;
        private readonly IEpicService _epicService;
        private static readonly int InvalidEpicId = -1;

        public ImportXlsxService(ITaskService taskService, IPanelService panelService, IEpicService epicService)
        {
            _taskService = taskService;
            _panelService = panelService;
            _epicService = epicService;
        }

        public async Task ImportTasksFromFile(IBrowserFile file, string userName)
        {
            LicenseContext licenseContext = LicenseContext.NonCommercial;

            var logFilePath = $"ErroresImport-{userName}.txt";
            using var logFile = new StreamWriter(logFilePath, append: true);


            using var memoryStream = new MemoryStream();
            await file.OpenReadStream().CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            using var package = new ExcelPackage(memoryStream);

            var worksheet = package.Workbook.Worksheets.First();
            var rowCount = worksheet.Dimension.Rows;

            for (int row = 1; row <= rowCount; row++)
            {
                var title = worksheet.Cells[row, 1].Text;
                var description = worksheet.Cells[row, 2].Text;
                string rowContent = $"Row {row}: {title}, {description}";

                if (!DateTime.TryParse(worksheet.Cells[row, 3].Text, out var dueDate))
                {
                    LogError(logFile, rowContent, "Invalid date format.");
                    continue;
                }

                if (!int.TryParse(worksheet.Cells[row, 4].Text, out var panelId))
                {
                    LogError(logFile, rowContent, "Panel ID not found.");
                    continue;
                }
                
                try
                {
                    _panelService.GetPanelById(panelId);
                }
                catch (System.Exception ex)
                {
                    LogError(logFile, rowContent, $"Panel ID not found: {ex.Message}");
                    continue;
                }

                if (!Enum.TryParse<EPriority>(worksheet.Cells[row, 5].Text, true, out var priority))
                {
                    LogError(logFile, rowContent, "Invalid priority value.");
                    continue;
                }

                int epicId = InvalidEpicId;
                if (!worksheet.Cells[row, 6].Text.Equals(""))
                {
                    if (!int.TryParse(worksheet.Cells[row, 6].Text, out epicId))
                    {
                        LogError(logFile, rowContent, "Invalid Epic ID.");
                        continue;   
                    }
                }

                if (!int.TryParse(worksheet.Cells[row, 7].Text, out var estimatedEffortHours))
                {
                    LogError(logFile, rowContent, "Invalid estimated effort hours.");
                    continue;
                }

                var task = new Entity.Task
                {
                    Title = title,
                    Description = description,
                    DueDate = dueDate,
                    PanelId = panelId,
                    Priority = priority,
                    EpicId = epicId == InvalidEpicId ? null : epicId,
                    InvertedEstimateHour = estimatedEffortHours
                };

                try
                {
                    _epicService.GetEpicById(epicId);
                }
                catch (System.Exception ex)
                {
                    LogError(logFile, rowContent, $"Epic does not exist: {ex.Message}");
                    continue;
                }

                try
                {
                    _taskService.CreateTask(task);
                }
                catch (System.Exception ex)
                {
                    LogError(logFile, rowContent, $"Failed to create task: {ex.Message}");
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