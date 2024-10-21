using League.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace League.Data.Repositories
{
    public class StaffRepository : GenericRepository<Staff>, IStaffRepository
    {
        private readonly DataContext _context;

        public StaffRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Staff>> GetStaffByClubAsync(int clubId)
        {
            return await _context.Staffs.Where(s => s.ClubId == clubId).ToListAsync();
        }
    }
}
