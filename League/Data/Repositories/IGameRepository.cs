using League.Data.Entities;

namespace League.Data.Repositories
{
    public interface IGameRepository : IGenericRepository<Game>
    {
        Task UpdateGameStatusAsync(Game game);

        Task<Game> GetNextGameAsync();

        Task<List<Game>> GetGamesToCloseAsync();

        Task<int> GetGamesPlayedAsync(int id);

        Task<int> GetGamesWonAsync(int id);

        Task<int> GetGamesLostAsync(int id);

        Task<int> GetGamesDrawnAsync(int id);

        Task<int> GetGoalsScoredAsync(int id);

        Task<int> GetGoalsConcededAsync(int id);

        Task<int> GetPointsAsync(int id);
    }
}