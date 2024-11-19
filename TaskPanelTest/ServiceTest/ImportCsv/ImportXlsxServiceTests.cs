using Microsoft.AspNetCore.Components.Forms;
using Moq;
using OfficeOpenXml;
using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Service;
using TaskPanelLibrary.Service.Interface;
using TaskPanelTest.ServiceTest.ImportCsv;
using Task = System.Threading.Tasks.Task;

namespace TaskPanelTest.ServiceTest.ImportXlsx
{
    [TestClass]
    public class ImportXlsxServiceTests
    {
        private Mock<ITaskService> _mockTaskService;
        private Mock<IPanelService> _mockPanelService;
        private Mock<IEpicService> _mockEpicService;
        private ImportXlsxService _importXlsxService;

        [TestInitialize]
        public void Setup()
        {
            _mockTaskService = new Mock<ITaskService>();
            _mockPanelService = new Mock<IPanelService>();
            _mockEpicService = new Mock<IEpicService>();
            _importXlsxService = new ImportXlsxService(
                _mockTaskService.Object,
                _mockPanelService.Object,
                _mockEpicService.Object
            );
        }

        [TestMethod]
        public async Task ImportTasksFromFile_ValidFile_ShouldCreateTasks()
        {
            // Arrange
            var fileName = $"taskImport-{Guid.NewGuid()}.xlsx";
            var mockFile = CreateAndGetXlsxFile(fileName);

            _mockPanelService.Setup(service => service.GetPanelById(It.IsAny<int>())).Returns(new Panel());
            _mockEpicService.Setup(service => service.GetEpicById(It.IsAny<int>())).Returns(new Epic());
            _mockTaskService.Setup(service => service.CreateTask(It.IsAny<TaskPanelLibrary.Entity.Task>()));

            // Act
            await _importXlsxService.ImportTasksFromFile(mockFile, "testUser");

            // Assert
            _mockTaskService.Verify(service => service.CreateTask(It.IsAny<TaskPanelLibrary.Entity.Task>()), Times.AtLeastOnce);
        }

        private IBrowserFile CreateAndGetXlsxFile(string fileName)
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            // Crear el archivo XLSX
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                worksheet.Cells[1, 1].Value = "Title";
                worksheet.Cells[1, 2].Value = "Description";
                worksheet.Cells[1, 3].Value = "DueDate";
                worksheet.Cells[1, 4].Value = "PanelId";
                worksheet.Cells[1, 5].Value = "Priority";
                worksheet.Cells[1, 6].Value = "EpicId";
                worksheet.Cells[1, 7].Value = "EstimatedEffortHours";

                worksheet.Cells[2, 1].Value = "Task1";
                worksheet.Cells[2, 2].Value = "Description1";
                worksheet.Cells[2, 3].Value = "2024-12-12";
                worksheet.Cells[2, 4].Value = 1;
                worksheet.Cells[2, 5].Value = "High";
                worksheet.Cells[2, 6].Value = 100;
                worksheet.Cells[2, 7].Value = 8;

                worksheet.Cells[3, 1].Value = "Task2";
                worksheet.Cells[3, 2].Value = "Description2";
                worksheet.Cells[3, 3].Value = "2024-11-11";
                worksheet.Cells[3, 4].Value = 2;
                worksheet.Cells[3, 5].Value = "Medium";
                worksheet.Cells[3, 6].Value = 200;
                worksheet.Cells[3, 7].Value = 10;

                package.Save();
            }

            var fileStream = File.OpenRead(filePath);
            return new TestBrowserFile(fileStream, fileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        [TestCleanup]
        public void Cleanup()
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, "taskImport.xlsx");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            var logFilePath = $"ErroresImport-testUser.txt";
            if (File.Exists(logFilePath))
            {
                File.Delete(logFilePath);
            }
        }

    }
}
