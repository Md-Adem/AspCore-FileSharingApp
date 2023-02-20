using FileSharingApp.Areas.Admin.Models;

namespace FileSharingApp.Areas.Admin.Services
{
    public interface IUserService
    {
        IQueryable<AdminUserViewModel> GetAll();

        IQueryable<AdminUserViewModel> GetBlockedUsers();

        IQueryable<AdminUserViewModel> Search(string term);

        Task<OperationResult> BlockUser(string userId);

        Task<int> UserRegistrationCount();

        Task<int> UserRegistrationCount(int month);

        Task Initialize();
    }
}
