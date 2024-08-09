namespace Booking.WebAPI.Models
{
    public static class Constants
    {
        public static class BusinessHours
        {
            public const int OpeningHour = 9;
            public const int ClosingHour = 16;
        }
        
        public static class ConcurrentBookings
        {
            public const int MaxConcurrentBookingsCount = 4;
        }

        public const int Minutes = 59;
    }
}
