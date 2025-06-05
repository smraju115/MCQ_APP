using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMCQ.Data;
using OnlineMCQ.Models;
using OnlineMCQ.ViewModels;
using System.Threading.Tasks;

namespace OnlineMCQ.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AdminController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
     
        public IActionResult AdminDashboard()
        {
            //All Question Count
            var totalQuestions = _context.Questions.Count();


            //Total Examer Count
            var totalTestTakers = _context.TestResults
                .Select(tr => tr.UserTest.Id)
                .Distinct()
                .Count();

            //Day wise Exmer Count
            var today = DateTime.Today;
            var dailyTestStats = _context.TestResults
                .Where(tr => tr.SubmittedAt.Date == today)
                .GroupBy(tr => tr.SubmittedAt.Date)
                .Select(g => new DailyTestInfo
                {
                    Date = g.Key,
                    Count = g.Select(x => x.UserTest.Id).Distinct().Count()
                })
                .ToList();

            //var dailyTestStats = _context.TestResults
            //    .GroupBy(tr => tr.SubmittedAt.Date)
            //    .Select(g => new DailyTestInfo
            //    {
            //        Date = g.Key,
            //        Count = g.Select(x => x.UserTest.Id).Distinct().Count()
            //    })
            //    .OrderByDescending(x => x.Date)
            //    .ToList();

            var dashboardViewModel = new AdminDashboardViewModel
            {
                TotalQuestions = totalQuestions,
                TotalTestTakers = totalTestTakers,
                DailyTestInfos = dailyTestStats
            };

            return View(dashboardViewModel);
        }




        public IActionResult SuperAdminDashboard()
        {
            return View();
        }

        //Noite Get Method For Admin View
        public async Task<IActionResult> NoticeList()
        {
            var notices = await _context.Notices.OrderByDescending(n=>n.CreatedAt).ToListAsync();  
            return View(notices);
        }

        [HttpGet]
        public IActionResult CreateNotice()
        {
            return View();
        }

        //Create Notice 
        [HttpPost]
        public async Task<IActionResult> CreateNotice(Notice model, IFormFile file)
        {
            if (!ModelState.IsValid || file == null)
            {
                ModelState.AddModelError("", "Please upload a file.");
                return View(model);
            }

            // Ensure uploads folder exists
            var uploadPath = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            // Generate unique filename
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadPath, fileName);

            // Save file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            model.FileName = file.FileName;
            model.FilePath = "/uploads/" + fileName;
            model.CreatedAt = DateTime.Now;

            _context.Notices.Add(model);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Notice created successfully.";
            return RedirectToAction("NoticeList");
        }


        //Delete Method
        public IActionResult DeleteNotice(int id)
        {
            var notice = _context.Notices.Find(id);
            if (notice != null)
            {
                var fullPath = Path.Combine(_env.WebRootPath, notice.FilePath.TrimStart('/'));
                if (System.IO.File.Exists(fullPath))
                    System.IO.File.Delete(fullPath);

                _context.Notices.Remove(notice);
                _context.SaveChanges();
            }

            return RedirectToAction("NoticeList");
        }

















    }
}
