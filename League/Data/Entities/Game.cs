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

        public Club VisitedClub { get; set; }


        [Required]
        public int VisitorClubId { get; set; }

        public Club VisitorClub { get; set; }


        [Display(Name = "Visited Club Goals")]
        public int? VisitedGoals { get; set; }


        [Display(Name = "Visitor Club Goals")]
        public int? VisitorGoals { get; set; }


        [Display(Name = "Visited Club Assigned Cards")]
        public int? VisitedAssignedCards { get; set; }


        [Display(Name = "Visitor Club Assigned Cards")]
        public int? VisitorAssignedCards { get; set; }
    }
}
