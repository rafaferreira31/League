using League.Data.Entities;
using League.Models;

namespace League.Helpers
{
    public interface IConverterHelper
    {
        Player ToPlayer(PlayerViewModel model, string path, bool isNew);

        Player ToPlayerViewModel(Player player);

        Club ToClub(ClubViewModel model, string path, bool isNew);

        Club ToClubViewModel(Club club);

    }
}
