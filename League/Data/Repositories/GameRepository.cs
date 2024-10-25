using League.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace League.Data.Repositories
{
    public class GameRepository : GenericRepository<Game>, IGameRepository
    {
        private readonly DataContext _context;

        public GameRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> GetGamesDrawnAsync(int id)
        {
            return await _context.Games
        .Where(g => (g.VisitedClubId == id || g.VisitorClubId == id) &&
                    g.VisitedGoals == g.VisitorGoals && g.Status != Game.GameStatus.Scheduled)
        .CountAsync();
        }

        public async Task<int> GetGamesLostAsync(int id)
        {
            return await _context.Games
                    .Where(g => (g.VisitedClubId == id && g.VisitedGoals < g.VisitorGoals) ||
                                (g.VisitorClubId == id && g.VisitorGoals < g.VisitedGoals))
                    .CountAsync();
        }

        public async Task<int> GetGamesPlayedAsync(int id)
        {
            return await _context.Games
                    .Where(g => g.VisitedClubId == id || g.VisitorClubId == id && (g.Status != Game.GameStatus.Scheduled && g.Status != Game.GameStatus.OnGoing))
                    .CountAsync();
        }

        public Task<List<Game>> GetGamesToCloseAsync()
        {
            var cutoffDate = DateTime.Now.AddDays(-1);
            return _context.Games
                .Where(g => g.GameDate < cutoffDate && g.Status == Game.GameStatus.Finished).ToListAsync();
        }

        public async Task<int> GetGamesWonAsync(int id)
        {
            return await _context.Games
                    .Where(g => (g.VisitedClubId == id && g.VisitedGoals > g.VisitorGoals) ||
                                (g.VisitorClubId == id && g.VisitorGoals > g.VisitedGoals))
                    .CountAsync();
        }

        public async Task<int> GetGoalsConcededAsync(int id)
        {
            return (int)await _context.Games
                .Where(g => g.VisitedClubId == id || g.VisitorClubId == id)
                .SumAsync(g => g.VisitedClubId == id ? g.VisitorGoals : g.VisitedGoals);
        }

        public async Task<int> GetGoalsScoredAsync(int id)
        {
            return (int)await _context.Games
                    .Where(g => g.VisitedClubId == id || g.VisitorClubId == id)
                    .SumAsync(g => g.VisitedClubId == id ? g.VisitedGoals : g.VisitorGoals);
        }

        public async Task<Game> GetNextGameAsync()
        {
            var now = DateTime.Now;

            return await _context.Games
                .Where(g => g.GameDate > now && g.Status != Game.GameStatus.Closed)
                .OrderBy(g => g.GameDate)
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetPointsAsync(int id)
        {
            int wins = await GetGamesWonAsync(id);
            int draws = await GetGamesDrawnAsync(id);
            return (wins * 3) + draws;
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
