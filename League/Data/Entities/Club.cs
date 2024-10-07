using System.ComponentModel.DataAnnotations;

namespace League.Data.Entities
{
    public class Club : IEntity
    {
        public int Id { get; set; }


        [Required]
        public string Name { get; set; }


        [Display(Name = "Club Emblem")]
        public string? ImageId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FoundingDate { get; set; }


        [Required]
        public string Stadium { get; set; }
    }
}
