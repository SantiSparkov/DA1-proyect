using Microsoft.AspNetCore.Components.Forms;
using Moq;
using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Service;
using TaskPanelLibrary.Service.Interface;
using Task = System.Threading.Tasks.Task;

namespace TaskPanelTest.ServiceTest.ImportCsv
{
    [TestClass]
    public class ImportCsvServiceTests
    {
        private Mock<ITaskService> _mockTaskService;
        private Mock<IPanelService> _mockPanelService;
        private ImportCsvService _importCsvService;

        [TestInitialize]
        public void Setup()
        {
            _mockTaskService = new Mock<ITaskService>();
            _mockPanelService = new Mock<IPanelService>();
            _importCsvService = new ImportCsvService(_mockTaskService.Object, _mockPanelService.Object);
        }

        private IBrowserFile CreateAndGetCsvFile(string fileName)
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, fileName);

            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Title,Description,DueDate,PanelId,Priority");
                writer.WriteLine("Task1,Description1,2024-12-12,1,High");
                writer.WriteLine("Task2,Description2,2024-11-11,2,Medium");
            }

            var fileStream = File.OpenRead(filePath);
            return new TestBrowserFile(fileStream, fileName, "text/csv");
        }

        [TestMethod]
        public async Task ImportTasksFromFile_ValidFile_ShouldCreateTasks()
        {
            // Arrange
            var mockFile = CreateAndGetCsvFile("taskImport.csv");

            _mockPanelService.Setup(service => service.GetPanelById(It.IsAny<int>())).Returns(new Panel());
            _mockTaskService.Setup(service => service.CreateTask(It.IsAny<TaskPanelLibrary.Entity.Task>()));

            // Act
            await _importCsvService.ImportTasksFromFile(mockFile, "testUser");

            // Assert
            _mockTaskService.Verify(service => service.CreateTask(It.IsAny<TaskPanelLibrary.Entity.Task>()), Times.AtLeastOnce);
        }

        [TestCleanup]
        public void Cleanup()
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, "taskImport.csv");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
