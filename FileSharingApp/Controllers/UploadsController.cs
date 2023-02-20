using FileSharingApp.Data;
using FileSharingApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FileSharingApp.Controllers
{
    [Authorize]
    public class UploadsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _env;

        //IsBlocked = table.Column<bool>(type: "bit", nullable: false),
        //CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: DateTime.Now),
       // UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
       // FirstName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),

        public UploadsController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _db = context;
            _env = env; 
        }

        private string UserId
        {
            get { return User.FindFirstValue(ClaimTypes.NameIdentifier); }
        }

        public IActionResult Index()
        {
            // var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = _db.Uploads.Where(u => u.UserId == UserId)
                .OrderByDescending(u => u.UploadDate)
                .Select(u => new UploadsViewModel
                {
                    Id = u.Id,
                    FileName = u.FileName,
                    OriginalFileName = u.OriginalFileName,
                    ContentType = u.ContentType,
                    Size = u.Size,
                    UploadDate = u.UploadDate,
                    DownloadCount = u.DownloadCount,
                });

                return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]  
        public async Task<IActionResult> Create(InputUploads model)
        {
            if (ModelState.IsValid)
            {
                var newName = Guid.NewGuid().ToString();
                var extension = Path.GetExtension(model.File.FileName);
                var fileName = string.Concat(newName, extension);
                var root = _env.WebRootPath;
                var path = Path.Combine(root,"Uploads", fileName);

                using(var Filestream = System.IO.File.Create(path))
                {
                    await model.File.CopyToAsync(Filestream);
                }

                await _db.Uploads.AddAsync(new Uploads
                {
                    OriginalFileName = model.File.FileName,
                    FileName = fileName,
                    ContentType = model.File.ContentType,
                    Size = model.File.Length,
                    UserId = UserId,
                });

                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var selectedUpload = await _db.Uploads.FindAsync(id);
            if (selectedUpload == null)
            {
                return NotFound();
            }
            // This Securing the Delete from deleting uploads if the userId not equal the selected items userId 
            if(selectedUpload.UserId != UserId) 
            {
                return NotFound();
            }
            return View(selectedUpload);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmation(string id)
        {
            var selectedUpload = await _db.Uploads.FindAsync(id);
            if (selectedUpload == null)
            {
                return NotFound();
            }
            if (selectedUpload.UserId != UserId)
            {
                return NotFound();
            }

            _db.Uploads.Remove(selectedUpload);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        [HttpPost]
        [AllowAnonymous]
        public IActionResult Results(string search)
        {
            var model = _db.Uploads.Where(u => u.OriginalFileName.Contains(search)).Select(u => new UploadsViewModel
            {
                FileName = u.FileName,
                OriginalFileName = u.OriginalFileName,
                ContentType = u.ContentType,
                Size = u.Size,
                UploadDate = u.UploadDate,
                DownloadCount = u.DownloadCount,
            });


            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Browse(int requiredPage = 1)
        {

            // Pagination 

            const int pageSize = 6;

            decimal rowsCount = await _db.Uploads.CountAsync();

            var pagesCount = Math.Ceiling(rowsCount / pageSize);

            requiredPage = requiredPage > pagesCount ? 1 : requiredPage;
            requiredPage = requiredPage <= 0 ? 1 : requiredPage;

            int skipCount = (requiredPage - 1) * pageSize;

            // End

            var model = await _db.Uploads.OrderByDescending(u => u.DownloadCount).Select(u => new UploadsViewModel
            {
                FileName = u.FileName,
                OriginalFileName = u.OriginalFileName,
                ContentType = u.ContentType,
                Size = u.Size,
                UploadDate = u.UploadDate,
                DownloadCount = u.DownloadCount,
            })
                .Skip(skipCount)
                .Take(pageSize)
                .ToListAsync();
            ViewBag.CurrentPage = requiredPage;


            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Download(string id)
        {
            var selectedFile = await _db.Uploads.FirstOrDefaultAsync(u => u.FileName == id);
            if (selectedFile == null) 
            {
                return NotFound();
            }

            selectedFile.DownloadCount++;

            _db.Uploads.Update(selectedFile);
            await _db.SaveChangesAsync();

            var path = "~/Uploads/" + selectedFile.FileName;

            // this clear the cache from the broweser
            Response.Headers.Add("Expires", DateTime.Now.AddDays(-3).ToLongDateString());
            Response.Headers.Add("Cache-Control","no-cache");

            return File(path,selectedFile.ContentType,selectedFile.OriginalFileName);
        }
    }
}
