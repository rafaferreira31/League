using League.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace League.Models
{
    public class StaffViewModel : Staff
    {
        [Display(Name = "Profile Picture")]
        public IFormFile? ImageFile { get; set; }
    }
}
