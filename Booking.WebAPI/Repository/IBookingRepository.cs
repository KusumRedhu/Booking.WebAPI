using Booking.WebAPI.Models;

namespace Booking.WebAPI.Repository
{
    public interface IBookingRepository
    {
        Task<List<BookingDetail>> GetByTimeAsync(TimeSpan bookingStartTime, TimeSpan bookingEndTime);
        Task CreateAsync(BookingDetail bookingDetail);
    }
}
