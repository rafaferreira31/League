using League.Data.Entities;

namespace League.Data.Repositories
{
    public interface IClubRepository : IGenericRepository<Club>
    {
        Task<Club> GetByNameAsync(string name);
    }
}
