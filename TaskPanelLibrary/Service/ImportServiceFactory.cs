using TaskPanelLibrary.Service.Interface;

namespace TaskPanelLibrary.Service
{
    public class ImportServiceFactory
    {
        private readonly ImportCsvService _csvService;
        private readonly ImportXlsxService _xlsxService;

        public ImportServiceFactory(ImportCsvService csvService, ImportXlsxService xlsxService)
        {
            _csvService = csvService;
            _xlsxService = xlsxService;
        }

        public IImportService GetImportService(string fileType)
        {
            return fileType.ToLower() switch
            {
                ".csv" => _csvService,
                ".xlsx" => _xlsxService,
                _ => throw new NotSupportedException($"Unsupported file type: {fileType}")
            };
        }
    }
}