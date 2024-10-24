using League.Data.Entities;

namespace League.Data.Repositories
{
    public interface IGameRepository : IGenericRepository<Game>
    {
        Task UpdateGameStatusAsync(Game game);

        Task<List<Game>> GetGamesToCloseAsync();

        Task<int> GetGamesPlayedByClub(int clubId);

        Task<int> GetGamesWonByClub(int clubId);

        Task<int> GetGamesLostByClub(int clubId);

        Task<int> GetGamesDrawnByClub(int clubId);

        Task<int> GetGoalsScoredByClub(int clubId);

        Task<int> GetGoalsConcededByClub(int clubId);

        Task<int> CalculatePointsByClub(int clubId);

        Task<Game> GetNextGameAsync();
    }
}
