using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YasinFinal.DAL;
using YasinFinal.Models;

namespace YasinFinal.Areas.Bizlandadmin.Controllers
{
    [Area("Bizlandadmin")]

    public class TeamController : Controller
    {
        readonly AppDbContext _context;
        readonly IWebHostEnvironment _env;
        public TeamController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            List<Team> team = await _context.teams.ToListAsync();
            return View(team);
        }
        
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult>Create(Team team)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _context.teams.FirstOrDefaultAsync(x=>x.Id==team.Id);
            if (result != null)
            {
                return BadRequest();
            }
            if (!team.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Photo", "Ancaq sekil gondere bilersiz ");
                return View();
            }
            if (!(team.Photo.Length <200 * 1024))
            {
                ModelState.AddModelError("Photo", "Sekil olcusu 200 kb dan yuxari ola bilmez  ");
                return View();
            }
            string filename =Guid.NewGuid().ToString() + team.Photo.FileName;
            string path = Path.Combine(_env.WebRootPath, "assets/img", filename);
            using (FileStream file = new FileStream(path,FileMode.Create))
            {
                await team.Photo.CopyToAsync(file);
            }
            team.Image = filename;

            await _context.teams.AddAsync(team);
            await _context.SaveChangesAsync();




            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int?id)
        {
            Team team = await _context.teams.FirstOrDefaultAsync(x=>x.Id == id);

            string path = Path.Combine(_env.WebRootPath, "assets/img", team.Image);
            if(!System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
          _context.Remove(team);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int? id)
        {
            if(id==null||id<1) return View();   
            var existed = await _context.teams.FirstOrDefaultAsync(x=>x.Id == id);
            if (existed == null)
            {
                return NotFound();
            }
            return View(existed);
         }
        [HttpPost]
        public async Task<IActionResult>Update(int? id, Team team)
        {
            Team existed = await _context.teams.FirstOrDefaultAsync(_ => _.Id == id);
            if (existed != null)
            {
                if (!team.Photo.ContentType.Contains("image/"))
                {
                    ModelState.AddModelError("Photo", "Ancaq sekil gondere bilersiz ");
                    return View();
                }
                if (!(team.Photo.Length < 200 * 1024))
                {
                    ModelState.AddModelError("Photo", "Sekil olcusu 200 kb dan yuxari ola bilmez  ");
                    return View();
                }
                string path = Path.Combine(_env.WebRootPath, "assets/img", existed.Image);
                if (!System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                string filename = Guid.NewGuid().ToString()+team.Photo.FileName;
                string newpath = Path.Combine(_env.WebRootPath,"assets/img" ,filename);
                using (FileStream file = new FileStream(newpath,FileMode.Create))
                {
                    await team.Photo.CopyToAsync(file);
                }
                existed.Image = filename;
            }
            existed.Name = team.Name;
            existed.Position = team.Position;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
