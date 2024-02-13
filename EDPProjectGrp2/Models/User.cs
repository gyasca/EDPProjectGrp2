using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EDPProjectGrp2.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string RoleName { get; set; } = string.Empty;
        [Required]
        public string MembershipStatus { get; set; } = string.Empty;
        [Required]
        public string MobileNumber { get; set; } = string.Empty;
        [Required, EmailAddress, MaxLength(50)]
        public string Email { get; set; } = string.Empty;
        [Required, MinLength(8)]
        public string Password { get; set; } = string.Empty;
        public string? ProfilePhotoFile { get; set; }
        [Required, MinLength(1), MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        // Attributes for Samuel
        public string OccupationType { get; set; } = string.Empty;
        // End of attributes for Samuel
        [MaxLength(100)]
        public string Address { get; set; } = string.Empty;
        [MaxLength(10)]
        public string PostalCode { get; set; } = string.Empty;
        [Required]
        public bool NewsletterSubscriptionStatus { get; set; } = false;
        [Required]
        public bool TwoFactorAuthStatus { get; set; } = false;
        [Required]
        public bool VerificationStatus { get; set; } = false;
        [Required]
        public bool GoogleAccountType { get; set; } = false;

        // Navigation property to represent the one-to-many relationship
        [JsonIgnore]
		public List<ForumPost>? ForumPosts { get; set; }

		[Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UpdatedAt { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DateOfBirth { get; set; }


        [JsonIgnore] // This prevents the token from being serialized in responses
        public string? ResetPasswordToken { get; set; }

        [JsonIgnore] // This prevents the token expiration date from being serialized in responses
        [Column(TypeName = "datetime")]
        public DateTime? ResetPasswordTokenExpiresAt { get; set; }
    }
}
