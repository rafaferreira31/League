using League.Data.Entities;

namespace League.Data.Repositories
{
    public class GameRepository : GenericRepository<Game>, IGameRepository
    {
        public GameRepository(DataContext context) : base(context)
        {
        }
    }
}
