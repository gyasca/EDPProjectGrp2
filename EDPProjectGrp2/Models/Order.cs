using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDPProjectGrp2.Models
{
    public class Order
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        [StringLength(255)]
        public string OrderStatus { get; set; }

        [Required]
        public decimal SubTotalAmount { get; set; }

        [Required]
        public decimal GstAmount { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        [Required]
        public int NoOfItems { get; set; }

        [Required]
        [StringLength(255)]
        public string OrderPaymentMethod { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
