using FileSharingApp.Models;

namespace FileSharingApp.Services
{
    public interface IUploadService
    {
        IQueryable<UploadsViewModel> GetAll();

        IQueryable<UploadsViewModel> GetBy(string userId);

        IQueryable<UploadsViewModel> Search(string term);

        Task Create(InputUploads model);

        Task<UploadsViewModel> Find(string id, string userId);

        Task Delete(string id);

        Task IncreamentDownloadCount(string id);
    }
}
