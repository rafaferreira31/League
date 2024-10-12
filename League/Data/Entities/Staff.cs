using System.ComponentModel.DataAnnotations;

namespace League.Data.Entities
{
    public class Staff : IEntity
    {
        public int Id { get; set; }


        [Required]
        public string FirstName { get; set; }


        [Required]
        public string LastName { get; set; }


        public string FullName => $"{FirstName} {LastName}";


        [Required]
        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }


        [Required]
        public string FunctionPerformed { get; set; }


        [Display(Name = "Profile Image")]
        public string? ImageUrl { get; set; }


        [Required]
        public int ClubId { get; set; }
    }
}
