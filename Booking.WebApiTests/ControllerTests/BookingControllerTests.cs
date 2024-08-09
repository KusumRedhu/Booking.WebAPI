using Bogus;
using Booking.WebAPI.Controllers;
using Booking.WebAPI.Models;
using Booking.WebAPI.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace Booking.WebApiTests.ControllerTests
{
    public class BookingControllerTests
    {
        private readonly Mock<IBookingService> _bookingServiceMock = new();
        private readonly BookingController _sut;

        public BookingControllerTests()
        {
            _sut = new BookingController(_bookingServiceMock.Object);
        }

        [Theory]
        [InlineData("09:00", "John Smith", true)]
        [InlineData("09:15", "John Smith", true)]
        [InlineData("09:30", "John Smith", true)]
        [InlineData("09:45", "John Smith", true)]
        public async Task CreateBooking__WhenValidData_ReturnsOkStatusWithBookingId(string bookingTimeStr, string name, bool isAvailable)
        {
            //Arrange
            var fakeBookingRequest = new BookingRequest { BookingTime = bookingTimeStr, Name = name };
            var bookingTime = TimeSpan.Parse(fakeBookingRequest.BookingTime);
            var bookingId = Guid.NewGuid();
            var expectedBookingDetail = new Faker<BookingDetail>()
                .RuleFor(_=>_.BookingId, bookingId)
                .RuleFor(_=>_.BookingStartTime, bookingTime)
                .Generate();

            _bookingServiceMock.Setup(b =>
            b.IsBookingTimeAvailable(bookingTime)).ReturnsAsync(isAvailable);

            _bookingServiceMock.Setup(b => 
            b.CreateBookingAsync(bookingTime, fakeBookingRequest.Name)).ReturnsAsync(expectedBookingDetail.BookingId);

            //Act
            var response = await _sut.CreateBooking(fakeBookingRequest);

            //Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(response.Result);
            var result = Assert.IsType<Guid>(okObjectResult.Value);

            result.Should().Be(bookingId);
            okObjectResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("08:30", "John Smith")]
        [InlineData("16:01", "John Smith")]
        [InlineData("20:30", "John Smith")]
        public async Task CreateBooking_WhenOutOfBusinessHoursBooking_ReturnsBadRequestStatus(string bookingTime, string name)
        {
            //Arrange
            var fakeBookingRequest = new BookingRequest { BookingTime = bookingTime, Name = name };
            
            //Act
            var response = await _sut.CreateBooking(fakeBookingRequest);

            //Assert
            var result = Assert.IsType<BadRequestObjectResult>(response.Result);
            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CreateBooking_WhenNoBookingTimeAvailable_ReturnsConflictStatus()
        {
            //Arrange
            var fakeBookingRequest = new BookingRequest { BookingTime = "09:00", Name = "Test" };
            _bookingServiceMock.Setup(b => b.IsBookingTimeAvailable(It.IsAny<TimeSpan>())).ReturnsAsync(false);

            //Act
            var response = await _sut.CreateBooking(fakeBookingRequest);

            //Assert
            var result = Assert.IsType<ConflictObjectResult>(response.Result);
            result.StatusCode.Should().Be((int)HttpStatusCode.Conflict);
        }
    }
}
