namespace League.Models
{
    public class ClubStatisticsViewModel
    {
        public int ClubId { get; set; }


        public string ClubName { get; set; }


        public int GamesPlayed { get; set; }


        public int GamesWon { get; set; }


        public int GamesDrawn { get; set; }


        public int GamesLost { get; set; }


        public int GoalsScored { get; set; }


        public int GoalsConceded { get; set; }


        public int Points { get; set; }


        public string ImageFullPath { get; set; }
    }
}
