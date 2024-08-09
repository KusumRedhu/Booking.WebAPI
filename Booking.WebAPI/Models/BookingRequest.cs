using System.ComponentModel.DataAnnotations;

namespace Booking.WebAPI.Models
{
    public class BookingRequest
    {
        [Required]
        [RegularExpression(@"^([01]\d|2[0-3]):([0-5]\d)$", ErrorMessage = "Invalid time format")]
        public required string BookingTime { get; set; }
        [Required]
        [MinLength(1, ErrorMessage = "Name cannot be empty")]
        public required string Name { get; set; }
    }
}
