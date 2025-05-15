using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMCQ.Data;
using System.Threading.Tasks;

namespace OnlineMCQ.Controllers
{
    public class NoticeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NoticeController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Notie View Public
        public async Task<IActionResult> PublicNotices()
        {
            var notices = await _context.Notices.OrderByDescending(n => n.CreatedAt).ToListAsync();
            return View(notices);
        }
    }
}
