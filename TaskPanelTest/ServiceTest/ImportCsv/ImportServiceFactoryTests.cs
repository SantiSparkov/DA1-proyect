using Moq;
using TaskPanelLibrary.Service;

namespace TaskPanelTest.ServiceTest
{
    [TestClass]
    public class ImportServiceFactoryTests
    {
        private Mock<ImportCsvService> _mockCsvService;
        private Mock<ImportXlsxService> _mockXlsxService;
        private ImportServiceFactory _factory;

        [TestInitialize]
        public void Setup()
        {
            _mockCsvService = new Mock<ImportCsvService>(null, null, null);
            _mockXlsxService = new Mock<ImportXlsxService>(null, null, null);

            _factory = new ImportServiceFactory(_mockCsvService.Object, _mockXlsxService.Object);
        }

        [TestMethod]
        public void GetImportService_ValidCsvFileType_ShouldReturnCsvService()
        {
            // Act
            var service = _factory.GetImportService(".csv");

            // Assert
            Assert.AreEqual(_mockCsvService.Object, service);
        }

        [TestMethod]
        public void GetImportService_ValidXlsxFileType_ShouldReturnXlsxService()
        {
            // Act
            var service = _factory.GetImportService(".xlsx");

            // Assert
            Assert.AreEqual(_mockXlsxService.Object, service);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetImportService_InvalidFileType_ShouldThrowNotSupportedException()
        {
            // Act
            _factory.GetImportService(".txt");
        }
    }
}