using DocumentFormat.OpenXml.Spreadsheet;
using FileSharingApp.Areas.Admin.Services;
using FileSharingApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FileSharingApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly IContactUsService contactUsService;
        private readonly ApplicationDbContext context;

        public HomeController(IContactUsService contactUsService, ApplicationDbContext context)
        {
            this.contactUsService = contactUsService;
            this.context = context;
        }




        public async Task<IActionResult> Index()
        {
            var result = await contactUsService.GetAll().OrderByDescending(u => u.SentDate).ToListAsync();

            var totalUploads = await context.Uploads.CountAsync();
            ViewBag.TotalUploads = totalUploads;

            var totalUsers = await context.Users.CountAsync();
            ViewBag.TotalUsers = totalUsers;

            var blockedUsers = await context.Users.Where(u => u.IsBlocked == true).CountAsync();
            ViewBag.BlockedUsers = blockedUsers;

            var totalMessages = result.Count();
            ViewBag.TotalMessages = totalMessages;

            return View(result);
        }


        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int id)
        {
           
            await contactUsService.ChangeStatus(id);
           
            return RedirectToAction("Index");
            
        }
    }
}
