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

        private IBrowserFile GetCsvFileFromResources(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "resources", fileName);
            var fileStream = File.OpenRead(filePath);

            return new TestBrowserFile(fileStream, fileName, "text/csv");
        }

        [TestMethod]
        public async Task ImportTasksFromFile_ValidFile_ShouldCreateTasks()
        {
            // Arrange
            var mockFile = GetCsvFileFromResources("test.csv");

            _mockPanelService.Setup(service => service.FindById(It.IsAny<int>())).Returns(new Panel());
            _mockTaskService.Setup(service => service.CreateTask(It.IsAny<TaskPanelLibrary.Entity.Task>()));

            // Act
            await _importCsvService.ImportTasksFromFile(mockFile, "testUser");

            // Assert
            _mockTaskService.Verify(service => service.CreateTask(It.IsAny<TaskPanelLibrary.Entity.Task>()), Times.Once);
        }
    }
}