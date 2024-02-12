using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EDPProjectGrp2.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        [ForeignKey("Event")]
        public int EventId { get; set; }
        public Event Event { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public Order Order { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }
        [Required]
        public decimal Discounted { get; set; }
        [Required]
        public decimal DiscountedTotalPrice { get; set; }
    }
}