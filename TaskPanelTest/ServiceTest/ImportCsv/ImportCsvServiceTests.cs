using Microsoft.AspNetCore.Components.Forms;
using Moq;
using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Exception.Panel;
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
        private Mock<IEpicService> _mockEpicService;
        private ImportCsvService _importCsvService;

        [TestInitialize]
        public void Setup()
        {
            _mockTaskService = new Mock<ITaskService>();
            _mockPanelService = new Mock<IPanelService>();
            _mockEpicService = new Mock<IEpicService>();
            _importCsvService = new ImportCsvService(
                _mockTaskService.Object, 
                _mockPanelService.Object, 
                _mockEpicService.Object
            );
        }

        private IBrowserFile CreateAndGetCsvFile(string fileName)
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, fileName);

            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Task1,Description1,2024-12-12,1,High,100,8");
                writer.WriteLine("Task2,Description2,2024-11-11,2,Medium,200,10");
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
            _mockEpicService.Setup(service => service.GetEpicById(It.IsAny<int>())).Returns(new Epic());
            _mockTaskService.Setup(service => service.CreateTask(It.IsAny<TaskPanelLibrary.Entity.Task>()));

            // Act
            await _importCsvService.ImportTasksFromFile(mockFile, "testUser");

            // Assert
            _mockTaskService.Verify(service => service.CreateTask(It.IsAny<TaskPanelLibrary.Entity.Task>()), Times.AtLeastOnce);
        }
        
        [TestMethod]
        public async Task ImportTasksFromFile_InvalidFile_ShouldNotCreateTasks()
        {
            // Arrange
            var mockFile = CreateAndGetCsvFile("taskImport.csv");

            _mockPanelService.Setup(service => service.GetPanelById(It.IsAny<int>())).Throws(new PanelNotValidException("Panel not valid"));

            // Act
            await _importCsvService.ImportTasksFromFile(mockFile, "testUser");

            // Assert
            _mockTaskService.Verify(service => service.CreateTask(It.IsAny<TaskPanelLibrary.Entity.Task>()), Times.Never);
        }

        [TestMethod]
        public async Task ImportTasksFromFile_InvalidEpicId_ShouldLogError()
        {
            // Arrange
            var mockFile = CreateAndGetCsvFile("taskImport.csv");

            _mockPanelService.Setup(service => service.GetPanelById(It.IsAny<int>())).Returns(new Panel());
            _mockEpicService.Setup(service => service.GetEpicById(It.IsAny<int>()))
                            .Throws(new Exception("Epic does not exist."));
            var logFilePath = $"ErroresImport-testUser.txt";

            // Act
            await _importCsvService.ImportTasksFromFile(mockFile, "testUser");

            // Assert
            Assert.IsTrue(File.Exists(logFilePath));
        }

        [TestCleanup]
        public void Cleanup()
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, "taskImport.csv");
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
