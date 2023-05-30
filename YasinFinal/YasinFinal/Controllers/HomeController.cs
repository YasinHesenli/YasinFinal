using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YasinFinal.DAL;
using YasinFinal.Models;

namespace YasinFinal.Controllers
{
    public class HomeController : Controller
    {
        readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task< IActionResult> Index()
        {
            List<Team> teams = await _context.teams.ToListAsync();
            return View(teams);
        }
    }
}
