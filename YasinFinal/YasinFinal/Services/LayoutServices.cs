using Microsoft.EntityFrameworkCore;
using YasinFinal.DAL;

namespace YasinFinal.Services
{
    public class LayoutServices
    {
        readonly AppDbContext _context;

        public LayoutServices(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Dictionary<string, string>> Getsettings()
        {
            var settings = await _context.settings.ToDictionaryAsync(x=>x.Key,x=>x.Value);
            return settings;
        }
    }
}
