using League.Data.Entities;

namespace League.Data.Repositories
{
    public class PlayerRepository : GenericRepository<Player>, IPlayerRepository
    {
        public PlayerRepository(DataContext context) : base(context)
        {
        }
    }
}
