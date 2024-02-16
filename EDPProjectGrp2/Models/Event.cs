using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDPProjectGrp2.Models
{
    public class Event
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string EventName { get; set; } = string.Empty;

        [Required]
        [StringLength(100000)]
        public string EventDescription { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string EventCategory { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string EventLocation { get; set; } = string.Empty;

        [Required]
        [Range(1, 999)]
        public int EventTicketStock { get; set; } = 0;

        public string EventPicture { get; set; }

        [Required]
        [Range(0, 999)]
        public decimal EventPrice { get; set; } = 0;

        [Required]
        [Range(0, 999)]
        public decimal EventDiscountPrice { get; set; } = 0;

        [Range(0, 999)]
        public decimal EventUplayMemberPrice { get; set; } = 0;

        [Range(0, 999)]
        public decimal EventNtucClubPrice { get; set; } = 0; 

        [Required]
        public DateTime EventDate { get; set; } = DateTime.MinValue;

        [Required]
        public DateTime EventEndDate { get; set; } = DateTime.MinValue;

        [Required]
        [Range(0, 999)]
        public int EventDuration { get; set; } = 0;

        [Required]
        public bool EventSale { get; set; } = false;

        [Required]
        public bool EventStatus { get; set; } = true;
    }
}
