using League.Data.Entities;

namespace League.Data.Repositories
{
    public class GameRepository : GenericRepository<Game>, IGameRepository
    {
        private readonly DataContext _context;

        public GameRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task UpdateGameStatusAsync(Game game)
        {
            if(game.GameDate < DateTime.Now)
            {
                game.Status = Game.GameStatus.Finished;
            }
            else if(game.GameDate == DateTime.Now) 
            {
                game.Status = Game.GameStatus.OnGoing;
            }
            else
            {
                game.Status = Game.GameStatus.Scheduled;
            }
        }
    }
}
