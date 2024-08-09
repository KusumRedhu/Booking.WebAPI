using Booking.WebAPI.Models;

namespace Booking.WebAPI.Services
{
    public interface IBookingService
    {
        Task<Guid?> CreateBookingAsync(TimeSpan bookingTime, string name);
        Task<bool> IsBookingTimeAvailable(TimeSpan bookingStartTime);
    }
}
