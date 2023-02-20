using FileSharingApp.Data;
using FileSharingApp.Helpers.Mail;
using FileSharingApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;

namespace FileSharingApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IMailHelper _mailHelper;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IMailHelper mailHelper)
        {
            _logger = logger;
            _db = context;
			_mailHelper = mailHelper;
        }

        private string UserId
        {
            get
            {
                return User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var PopularDownload = _db.Uploads.OrderByDescending(u => u.DownloadCount)
                .Select(u => new UploadsViewModel
                {
                    Id = u.Id,
                    FileName = u.FileName,
                    OriginalFileName = u.OriginalFileName,
                    ContentType = u.ContentType,
                    Size = u.Size,
                    UploadDate = u.UploadDate,
                    DownloadCount = u.DownloadCount,
                }).Take(3);

            ViewBag.PopularDownload = PopularDownload;

            // i need to learn the session thing to make this work once for a user 
            // this will send email for every vist
            //_mailHelper.SendMail(new InputEmailMessage
            //{
            //    Email = "imohhx@gmail.com",
            //    Subject = "New Vistor",
            //    Body = "You Have a New Client",
            //});

            return View();
        }


        public IActionResult Profile()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Save to DB
                _db.Contact.Add(new Contact
                {
                    Name = model.Name,
                    Email = model.Email,
                    Message = model.Message,
                    Subject = model.Subject,
                    UserId = UserId
                });
                await _db.SaveChangesAsync();



				// Send Mail

				StringBuilder Body = new StringBuilder();

                Body.Append("Thank You For Your Intrest");

                Body.AppendLine("");
                Body.AppendLine("");

                Body.Append("We Will Contact You As Soon As Possible");

                Body.AppendLine("");
                Body.AppendLine("");

                Body.Append("This is Your Information");

                Body.AppendLine("");
                Body.AppendLine("");

                Body.Append("Your Name : " + model.Name);

				Body.AppendLine("");
				Body.AppendLine("");

				Body.Append("Your Email : " + model.Email);

				Body.AppendLine("");
				Body.AppendLine("");

				Body.Append("Your Message : " + model.Message);


                _mailHelper.SendMail(new InputEmailMessage
                {
                    Email = model.Email,
                    Subject = model.Subject,
                    Body = Body.ToString(),
				});
				TempData["Message"] = " Message has been sent successfully!";

				return RedirectToAction("Contact");
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [AllowAnonymous]
        public IActionResult SetCulture(string lang, string returnUrl)
        {
            if(!string.IsNullOrEmpty(lang))
            {
                Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(lang)),
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );
            }
            return LocalRedirect(returnUrl);
        }
    }
}