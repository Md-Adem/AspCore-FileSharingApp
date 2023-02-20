using DocumentFormat.OpenXml.Spreadsheet;
using FileSharingApp.Areas.Admin.Models;
using FileSharingApp.Data;
using Microsoft.AspNetCore.Identity;

namespace FileSharingApp.Areas.Admin.Services
{
    public class ContactUsService : IContactUsService
    {
        private readonly ApplicationDbContext context;

        public ContactUsService(ApplicationDbContext context)
        {
            this.context = context;
        }


        public IQueryable<ContactUsViewModel> GetAll()
        {
            var result = context.Contact.Select(u => new ContactUsViewModel
            {
                Id = u.Id,
                Name = u.Name,
                Email= u.Email,
                Message= u.Message,
                Subject = u.Subject,
                status = u.status,
                SentDate = u.SentDate
            });

            return result;
        }


        public async Task ChangeStatus(int id)
        {
            var selectedItem = await context.Contact.FindAsync(id);
            if (selectedItem != null)
            {
                // this will work like a toggle (true or false)
                selectedItem.status = !selectedItem.status;
                context.Update(selectedItem);
                await context.SaveChangesAsync();
            }
        }
    }
}
