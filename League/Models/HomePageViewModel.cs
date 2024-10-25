using League.Data.Entities;

namespace League.Models
{
    public class HomePageViewModel
    {
        public Game NextGame { get; set; }


        public List<ClubStatisticsViewModel>? ClubStatistics { get; set; }


        public List<Player>? RandomPlayers { get; set; }
    }
}
