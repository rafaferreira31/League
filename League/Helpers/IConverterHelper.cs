using League.Data.Entities;
using League.Models;

namespace League.Helpers
{
    public interface IConverterHelper
    {
        Player ToPlayer(PlayerViewModel model, Guid imageId, bool isNew);

        Player ToPlayerViewModel(Player player);

        Club ToClub(ClubViewModel model, Guid imageId , bool isNew);

        Club ToClubViewModel(Club club);

        Staff ToStaff(StaffViewModel model, Guid imageId, bool isNew);

        Staff ToStaffViewModel(Staff staff);

    }
}
