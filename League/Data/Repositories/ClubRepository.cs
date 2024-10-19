using League.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace League.Data.Repositories
{
    public class ClubRepository : GenericRepository<Club>, IClubRepository
    {
        private readonly DataContext _context;

        public ClubRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Club> GetByNameAsync(string name)
        {
            return await _context.Clubs
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Name == name);
        }

        public async Task<string> GetClubEmblemById(int id)
        {
            var club = await _context.Clubs.FirstOrDefaultAsync(c => c.Id == id);
            if(club == null)
            {
                return null;
            }

            return club.ImageFullPath;
        }

        public async Task<string> GetClubNameById(int id)
        {
            var club = await _context.Clubs
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            return club.Name;
        }
    }
}
