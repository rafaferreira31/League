using League.Data.Entities;

namespace League.Data.Repositories
{
    public interface IStaffRepository : IGenericRepository<Staff>
    {
        Task <List<Staff>> GetStaffByClubAsync (int clubId);
    }
}
