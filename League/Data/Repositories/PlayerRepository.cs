using League.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace League.Data.Repositories
{
    public class PlayerRepository : GenericRepository<Player>, IPlayerRepository
    {
        private readonly DataContext _context;

        public PlayerRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Player>> GetPlayersByClubAsync(int clubId)
        {
            return await _context.Players.Where(p => p.ClubId == clubId).ToListAsync();
        }
    }
}
