using Microsoft.EntityFrameworkCore;

namespace Booking.WebAPI.Models
{
    public class ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<BookingDetail> BookingDetail { get; set; }
    }
}
