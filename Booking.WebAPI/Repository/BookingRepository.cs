using Booking.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Booking.WebAPI.Repository
{
    public class BookingRepository(ApplicationDbContext dbContext) : IBookingRepository
    {
        public async Task<List<BookingDetail>> GetByTimeAsync(TimeSpan bookingStartTime, TimeSpan bookingEndTime)
        {
            var result = await dbContext.BookingDetail.
                Where(b => b.BookingEndTime >= bookingStartTime && b.BookingStartTime <= bookingEndTime).ToListAsync();           
            return result;
        }

        public async Task CreateAsync(BookingDetail bookingDetail)
        {
            await dbContext.AddAsync(bookingDetail);
            await dbContext.SaveChangesAsync();
        }
    }
}
