using System.ComponentModel.DataAnnotations;

namespace League.Data.Entities
{
    public class Player : IEntity
    {
        public int Id { get; set; }


        [Required]
        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters lenght.")]
        public string FirstName { get; set; }


        [Required]
        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters lenght.")]
        public string LastName { get; set; }


        public string FullName => $"{FirstName} {LastName}";


        [Required]
        [Display(Name = "Birth Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime BirthDate { get; set; }


        [Required]
        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters lenght.")]
        public string Position { get; set; }


        [Display(Name = "Profile Image")]
        public string? ImageUrl { get; set; }    
    }
}
