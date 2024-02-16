using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDPProjectGrp2.Models
{
    public class Refund
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime RequestRefundDate { get; set; }

        public DateTime? RefundDate { get; set; }

        [Required]
        public decimal RefundAmount { get; set; }

        [Required]
        public string RefundReason { get; set; }

        [Required]
        public string RefundStatus { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public Order Order { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
