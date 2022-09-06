using PlatformService.Models;

namespace PlatformService.Data
{
    public class PlatformRepo : IPlatformRepo
    {
        private readonly AppDbContext _context;
        public PlatformRepo(AppDbContext context)
        {
            _context = context;
        }
        public void CreatePlatform(Platform plat)
        {
            if (plat == null)
            {
                throw new ArgumentNullException(nameof(plat));
            }
            _context.Platforms.Add(plat);
        }

        public void DeletePlatform(int id)
        {
            var platform = GetPlatformById(id);
            if(platform==null)
                throw new Exception($"Data {id} tidak ditemukan");
            _context.Remove(platform);
            _context.SaveChanges();
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
           return _context.Platforms.ToList();
        }

        public IEnumerable<Platform> GetByName(string name)
        {
            return _context.Platforms.Where(p=>p.Name.ToLower().Contains(name.ToLower()));
        }

        public Platform GetPlatformById(int id)
        {
            return _context.Platforms.FirstOrDefault(p => p.Id == id);
        }

        public bool SaveChanges()
        {
           return (_context.SaveChanges() >= 0);
        }

        public void UpdatePlatform(Platform plat)
        {
           var platform = GetPlatformById(plat.Id);
           if(platform==null)
                throw new Exception($"Data id: {plat.Id} tidak ditemukan");

           platform.Name = plat.Name;
           platform.Publisher = plat.Publisher;
           platform.Cost = plat.Cost;
           _context.SaveChanges();
        }
    }
}