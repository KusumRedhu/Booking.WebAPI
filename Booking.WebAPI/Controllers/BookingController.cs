using Booking.WebAPI.Models;
using Booking.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using static Booking.WebAPI.Models.Constants;

namespace Booking.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]    
    public class BookingController(IBookingService bookingService) : ControllerBase
    {
        /// <summary>
        /// Post the BookingRequest
        /// </summary>
        /// <remarks>
        ///     Sample request : 
        ///     POST http://localhost:5200/api/booking
        ///     {
        ///         "bookingTime": "09:30",
        ///         "name":"John Smith"
        ///      }
        /// </remarks>
        /// Swagger url to follow - https://localhost:7029/swagger/index.html
        /// <param name="bookingRequest"></param>
        /// <returns>Ok status with bookingId created</returns>
        /// <response code="200">Ok status with bookingId returned</response>
        /// <response code="400">Invalid request model or out of office hours</response>
        /// <response code="409">No booking available at booking time</response>
        [HttpPost(Name = "CreateBooking")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Guid>> CreateBooking([FromBody] BookingRequest bookingRequest)
        {
            var bookingTime = TimeSpan.Parse(bookingRequest.BookingTime);

            if (bookingTime < TimeSpan.FromHours(BusinessHours.OpeningHour) || bookingTime > TimeSpan.FromHours(BusinessHours.ClosingHour))
            {
                return BadRequest("Out of office hours");
            }
            if (!(await bookingService.IsBookingTimeAvailable(bookingTime)))
            {
                return Conflict("No slot available for the booking time");
            } 

            var result = await bookingService.CreateBookingAsync(TimeSpan.Parse(bookingRequest.BookingTime), bookingRequest.Name);            
            
            return Ok(result);
        }
    }
}
