using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDPProjectGrp2.Models
{
    public class Ticket
    {
        [Key] // Marks the property as the primary key
        public int Id { get; set; }

        [Required] // Specifies that the property is required in the database
        public string Subject { get; set; }

        [Required]
        public string Description { get; set; }

        [StringLength(50)] // Sets the maximum length for the property
        public string Status { get; set; }

        // Other properties...

        [Column(TypeName = "datetime")] // Specifies the database column type
        public DateTime CreatedAt { get; set; }
    }
}
