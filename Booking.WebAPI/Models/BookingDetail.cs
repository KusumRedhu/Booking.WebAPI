using System.ComponentModel.DataAnnotations;

namespace Booking.WebAPI.Models
{
    public class BookingDetail
    {
        [Key]
        public Guid BookingId { get; set; }
        [Required]
        public TimeSpan BookingStartTime { get; set; }
        [Required]
        public TimeSpan BookingEndTime { get; set; }

        [Required]
        public required string CustomerName { get; set; }
    }
}
