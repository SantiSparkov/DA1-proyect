using Microsoft.AspNetCore.Components.Forms;

namespace TaskPanelLibrary.Service.Interface
{
    public interface IImportService
    {
        Task ImportTasksFromFile(IBrowserFile file, string userName);
    }
}