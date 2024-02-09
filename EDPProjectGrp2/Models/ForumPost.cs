using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EDPProjectGrp2.Models
{
	public class ForumPost
	{
		[Key]
		public int PostId { get; set; }

		[ForeignKey("User")]
		public int UserId { get; set; }

		public User? User { get; set; }

		[Required, MaxLength(255)]
		public string PostTopic { get; set; } = string.Empty;

		[Required, MaxLength(100)]
		public string Title { get; set; } = string.Empty;

		[Required]
		public string Content { get; set; } = string.Empty;

		public int Votes { get; set; }

		[Column(TypeName = "datetime")]
		public DateTime DateCreated { get; set; }

		[Column(TypeName = "datetime")]
		public DateTime? DateEdited { get; set; }
	}
}
