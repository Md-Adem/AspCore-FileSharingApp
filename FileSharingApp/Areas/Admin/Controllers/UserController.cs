using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using FileSharingApp.Areas.Admin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FileSharingApp.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class UserController : Controller
    {

        private readonly IUserService userService;
        private readonly IXLWorkbook workbook;

        public UserController(IUserService userService, IXLWorkbook workbook)
        {
            this.userService = userService;
            this.workbook = workbook;
        }


        // *****************************************************


        public async Task<IActionResult> Index()
        {

            var result = await userService.GetAll().ToListAsync();

            return View(result);
        }


        public async Task<IActionResult> Blocked()
        {

            var result = await userService.GetBlockedUsers().ToListAsync();

            return View(result);
        }


        [HttpPost]
        public async Task<IActionResult> Block(string userId)
        {

            if (!string.IsNullOrEmpty(userId))
            {
                var result = await userService.BlockUser(userId);

                if (result.Success)
                {
                    TempData["Success"] = result.Message;
                }
                else { TempData["Error"] = result.Message;  }

                return RedirectToAction("Index");
            }

            TempData["Error"] = "User Id is Not Found";
            return RedirectToAction("Index");
        }



        [HttpPost]
        public async Task<IActionResult> Search(string term)
        {

            if (ModelState.IsValid)
            {
                var result = await userService.Search(term).ToListAsync();

                return View("Index", result);
            }

            return RedirectToAction("Index");
            
        }


        [HttpPost]
        public async Task<IActionResult> UsersCount()
        {

            var totalUserCount = await userService.UserRegistrationCount();

            var month = DateTime.Today.Month;

            var monthUserCount = await userService.UserRegistrationCount(month);

            return Json(new {total = totalUserCount, month = monthUserCount});
        }





        public async Task<IActionResult> ExportToExcel()
        {
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            string fileName = "Users.xlsx";

            var result = await userService.GetAll().ToListAsync();

            //create worksheet with name "all users" inside workbook
            var worksheet = workbook.Worksheets.Add("All Users");

            // create the header for this sheet, in the row 1 culomn 1 & in row 1 culomn 2 etc...
            worksheet.Cell(1,1).Value = "First Name";
            worksheet.Cell(1, 2).Value = "Last Name";
            worksheet.Cell(1, 3).Value = "Email";
            worksheet.Cell(1, 4).Value = "Created Date";


            for (int i = 0; i < result.Count; i++)
            {
                var row = i + 2;
                worksheet.Cell(row, 1).Value = result[i].FirstName;
                worksheet.Cell(row, 2).Value = result[i].LastName;
                worksheet.Cell(row, 3).Value = result[i].Email;
                worksheet.Cell(row, 4).Value = result[i].CreatedDate;
            }

            using (var memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                return File(memoryStream.ToArray(),contentType, fileName);
            }
        }
    }
}
