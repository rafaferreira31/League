using League.Data.Entities;

namespace League.Data.Repositories
{
    public class ClubRepository : GenericRepository<Club>, IClubRepository
    {
        public ClubRepository(DataContext context) : base(context)
        {
        }
    }
}
