using Booking.WebAPI.Models;
using Booking.WebAPI.Repository;
using static Booking.WebAPI.Models.Constants;

namespace Booking.WebAPI.Services
{
    public class BookingService(IBookingRepository bookingRepository) : IBookingService
    {
        public async Task<Guid?> CreateBookingAsync(TimeSpan bookingTime, string name)
        {            
            var bookingEndTime = AddMinutesToTime(bookingTime);       
            var bookingDetail = new BookingDetail 
            {
                BookingId = Guid.NewGuid(),
                CustomerName = name,
                BookingStartTime = bookingTime,
                BookingEndTime = bookingEndTime,
            };
            await bookingRepository.CreateAsync(bookingDetail);
            return bookingDetail.BookingId;
        }
        
        public async Task<bool> IsBookingTimeAvailable(TimeSpan bookingTime)
        {
            var bookingEndTime = AddMinutesToTime(bookingTime);
            var totalCount = (await bookingRepository.GetByTimeAsync(bookingTime, bookingEndTime)).Count;
            if (totalCount >= ConcurrentBookings.MaxConcurrentBookingsCount) { return false; }
            return true;
        }

        private static TimeSpan AddMinutesToTime(TimeSpan time)
        {
            return time.Add(TimeSpan.FromMinutes(Minutes));
        }
    }
}
