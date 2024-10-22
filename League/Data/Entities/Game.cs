using System.ComponentModel.DataAnnotations;

namespace League.Data.Entities
{
    public class Game : IEntity
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Game Date")]
        public DateTime GameDate { get; set; }


        [Required]
        public int VisitedClubId { get; set; }


        [Display(Name = "Visited Club")]
        public string? VisitedClubName { get; set; }


        [Required]
        public int VisitorClubId { get; set; }


        [Display(Name = "Visitor Club")]
        public string? VisitorClubName { get; set; }


        [Display(Name = "Visited Club Goals")]
        public int? VisitedGoals { get; set; }


        [Display(Name = "Visitor Club Goals")]
        public int? VisitorGoals { get; set; }


        public GameStatus Status { get; set; }


        public enum GameStatus
        {
            Scheduled,
            OnGoing,
            Finished
        }
    }
}
