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
        public string VisitedClub { get; set; }


        [Required]
        public string VisitorClub { get; set; }


        [Required]
        [Display(Name = "Visited Club Goals")]
        public int? VisitedGoals { get; set; }


        [Required]
        [Display(Name = "Visitor Club Goals")]
        public int? VisitorGoals { get; set; }
    }
}
