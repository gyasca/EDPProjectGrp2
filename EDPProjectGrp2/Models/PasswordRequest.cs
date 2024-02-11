using System.ComponentModel.DataAnnotations;

namespace EDPProjectGrp2.Models
{
    public class PasswordRequest
    {
        [Required, MinLength(8)]
        public string Password { get; set; } = string.Empty;
    }
}
