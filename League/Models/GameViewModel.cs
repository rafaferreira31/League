namespace League.Models
{
    public class GameViewModel
    {

        public int Id { get; set; }
        public DateTime GameDate { get; set; }
        public string? VisitedClubName { get; set; }
        public string? VisitorClubName { get; set; }
        public int? VisitedGoals { get; set; }
        public int? VisitorGoals { get; set; }
        public string VisitedClubEmblem { get; set; }
        public string VisitorClubEmblem { get; set; }
        public League.Data.Entities.Game.GameStatus Status { get; set; }


    }
}
