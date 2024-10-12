using League.Data.Entities;

namespace League.Data.Repositories
{
    public class StaffRepository : GenericRepository<Staff>, IStaffRepository
    {
        public StaffRepository(DataContext context) : base(context)
        {
        }
    }
}
