using FileSharingApp.Data;
using FileSharingApp.Models;

namespace FileSharingApp.Services
{
    public class UploadService : IUploadService
    {
        private readonly ApplicationDbContext _db;
        public UploadService(ApplicationDbContext db) 
        {
            _db= db;
        }

        // To be Complete 
        public Task Create(InputUploads model)
        {
            // To be Complete 

            throw new NotImplementedException();
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<UploadsViewModel> Find(string id, string userId)
        {
            throw new NotImplementedException();
        }

        public IQueryable<UploadsViewModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<UploadsViewModel> GetBy(string userId)
        {
            throw new NotImplementedException();
        }

        public Task IncreamentDownloadCount(string id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<UploadsViewModel> Search(string term)
        {
            throw new NotImplementedException();
        }
    }
}
