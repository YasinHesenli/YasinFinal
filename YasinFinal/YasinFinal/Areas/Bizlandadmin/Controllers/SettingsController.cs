using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YasinFinal.DAL;
using YasinFinal.Models;

namespace YasinFinal.Areas.Bizlandadmin.Controllers
{
    [Area("Bizlandadmin")]
    public class SettingsController : Controller
    {
        readonly AppDbContext _context;

        public SettingsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task< IActionResult> Index()
        {
            var settings = await _context.settings.ToListAsync();
            return View(settings);
        }

        public IActionResult Update()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Update(Settings setting)
        {
            Settings existed = await _context.settings.FirstOrDefaultAsync(x=>x.Id == setting.Id);
            
            if(setting == null)
            {
                return View();
            }
            if(!ModelState.IsValid)
            {
                return View();
            }
            existed.Value=setting.Value;
           

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
       
    }
}
