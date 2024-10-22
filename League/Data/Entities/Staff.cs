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


        [Display(Name ="Full Name")]
        public string FullName => $"{FirstName} {LastName}";


        [Required]
        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }


        [Required]
        [Display(Name = "Function")]
        public string FunctionPerformed { get; set; }


        [Display(Name = "Profile Image")]
        public Guid ImageId { get; set; }


        [Required]
        public int ClubId { get; set; }


        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://nosleague.azurewebsites.net/images/noimage.png"
            : $"https://nosleague.blob.core.windows.net/staffs/{ImageId}";
    }
}