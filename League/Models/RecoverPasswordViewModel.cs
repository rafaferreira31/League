using System.ComponentModel.DataAnnotations;

namespace League.Models
{
    public class RecoverPasswordViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
