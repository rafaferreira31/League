using System.ComponentModel.DataAnnotations;

namespace League.Data.Entities
{
    public class Club : IEntity
    {
        public int Id { get; set; }


        [Required]
        public string Name { get; set; }


        [Display(Name = "Club Emblem")]
        public Guid ImageId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FoundingDate { get; set; }


        [Required]
        public string Stadium { get; set; }


        public ICollection<Player>? Players { get; set; }

        public ICollection<Staff>? Staffs { get; set; }


        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://nosleague.azurewebsites.net/images/noimage.png"
            : $"https://nosleague.blob.core.windows.net/clubs/{ImageId}";


        public int? GamesPlayed { get; set; }

        public int? GamesWon { get; set; }

        public int? GamesDrawn { get; set; }

        public int? GamesLost { get; set; }

        public int? GoalsScored { get; set; }

        public int? GoalsConceded { get; set; }

        public int? Points { get; set; }
    }
}

