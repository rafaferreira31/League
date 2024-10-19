using League.Data.Entities;

namespace League.Data.Repositories
{
    public interface IGameRepository : IGenericRepository<Game>
    {
        Task UpdateGameStatusAsync(Game game);

    }
}
