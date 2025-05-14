using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineMCQ.Data;
using OnlineMCQ.ViewModels;

namespace OnlineMCQ.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AdminController(ApplicationDbContext context)
        {
            _context = context;
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


    }
}
