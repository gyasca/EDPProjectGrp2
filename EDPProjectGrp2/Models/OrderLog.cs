using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EDPProjectGrp2.Models
{
    public class OrderLog
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        [StringLength(255)]
        public string ChangedStatus { get; set; }
    }
}

