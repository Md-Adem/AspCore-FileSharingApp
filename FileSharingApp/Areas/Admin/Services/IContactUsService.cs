using FileSharingApp.Areas.Admin.Models;

namespace FileSharingApp.Areas.Admin.Services
{
    public interface IContactUsService
    {
        IQueryable<ContactUsViewModel> GetAll();

        Task ChangeStatus(int id);
    }
}
