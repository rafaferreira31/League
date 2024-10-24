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

        public async Task<int> CalculatePointsByClub(int clubId)
        {
            var wins = await GetGamesWonByClub(clubId);
            var draws = await GetGamesDrawnByClub(clubId);
            return (wins * 3) + draws;
        }

        public async Task<int> GetGamesDrawnByClub(int clubId)
        {
            return await _context.Games
               .CountAsync(g => (g.VisitedClubId == clubId || g.VisitorClubId == clubId)
                                 && g.VisitedGoals == g.VisitorGoals
                                 && (g.Status == Game.GameStatus.Finished || g.Status == Game.GameStatus.Closed));
        }

        public async Task<int> GetGamesLostByClub(int clubId)
        {
            return await _context.Games
                           .CountAsync(g => (g.VisitedClubId == clubId && g.VisitedGoals < g.VisitorGoals) ||
                                            (g.VisitorClubId == clubId && g.VisitorGoals < g.VisitedGoals)
                                            && (g.Status == Game.GameStatus.Finished || g.Status == Game.GameStatus.Closed));
        }

        public async Task<int> GetGamesPlayedByClub(int clubId)
        {
            return await _context.Games.CountAsync(g => ((g.VisitedClubId == clubId || g.VisitorClubId == clubId)) && (g.Status == Game.GameStatus.Finished || g.Status == Game.GameStatus.Closed));
        }

        public Task<List<Game>> GetGamesToCloseAsync()
        {
            var cutoffDate = DateTime.Now.AddDays(-1);
            return _context.Games
                .Where(g => g.GameDate < cutoffDate && g.Status == Game.GameStatus.Finished).OrderByDescending(g => g.GameDate).ToListAsync();
        }

        public async Task<int> GetGamesWonByClub(int clubId)
        {
            return await _context.Games
                 .CountAsync(g => (g.VisitedClubId == clubId && g.VisitedGoals > g.VisitorGoals) ||
                                 (g.VisitorClubId == clubId && g.VisitorGoals > g.VisitedGoals)
                                 && g.Status == Game.GameStatus.Finished);
        }

        public async Task<int> GetGoalsConcededByClub(int clubId)
        {
            var visitedConceded = await _context.Games
                .Where(g => g.VisitedClubId == clubId && (g.Status == Game.GameStatus.Finished || g.Status == Game.GameStatus.Closed))
                .SumAsync(g => g.VisitorGoals ?? 0);

            var visitorConceded = await _context.Games
                .Where(g => g.VisitorClubId == clubId && (g.Status == Game.GameStatus.Finished || g.Status == Game.GameStatus.Closed))
                .SumAsync(g => g.VisitedGoals ?? 0);

            return visitedConceded + visitorConceded;
        }

        public async Task<int> GetGoalsScoredByClub(int clubId)
        {
            var visitedGoals = await _context.Games
                            .Where(g => g.VisitedClubId == clubId && (g.Status == Game.GameStatus.Finished || g.Status == Game.GameStatus.Closed))
                            .SumAsync(g => g.VisitedGoals ?? 0);

            var visitorGoals = await _context.Games
                .Where(g => g.VisitorClubId == clubId && (g.Status == Game.GameStatus.Finished || g.Status == Game.GameStatus.Closed))
                .SumAsync(g => g.VisitorGoals ?? 0);

            return visitedGoals + visitorGoals;
        }

        public async Task<Game> GetNextGameAsync()
        {
            var now = DateTime.Now;

            return await _context.Games
                .Where(g => g.GameDate > now && g.Status != Game.GameStatus.Closed)
                .OrderBy(g => g.GameDate)
                .FirstOrDefaultAsync();
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
