using League.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace League.Models
{
    public class ClubViewModel : Club
    {
        [Display(Name = "Club Emblem")]
        public IFormFile ImageFile { get; set; }
    }
}
