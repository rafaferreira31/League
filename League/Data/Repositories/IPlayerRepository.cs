using League.Data.Entities;

namespace League.Data.Repositories
{
    public interface IPlayerRepository : IGenericRepository<Player>
    {
        Task <List<Player>> GetPlayersByClubAsync(int clubId);
    }
}
