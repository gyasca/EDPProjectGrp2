// ResetPasswordRequest.cs

using System.ComponentModel.DataAnnotations;

public class ResetPasswordRequest
{
    [Required]
    public string Token { get; set; }

    [Required]
    [MinLength(8)] // Adjust the minimum length as needed
    public string NewPassword { get; set; }
}
