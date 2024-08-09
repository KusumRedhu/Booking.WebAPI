using Bogus;
using Booking.WebAPI.Models;
using Booking.WebAPI.Repository;
using Booking.WebAPI.Services;
using FluentAssertions;
using Moq;

namespace Booking.WebApiTests.ServiceTests
{
    public class BookingServiceTests
    {
        private readonly Mock<IBookingRepository> _bookingRepositoryMock = new();

        [Fact]
        public async Task CreateBooking_ReturnsGuid()
        {
            //Arrange
            var bookingId = Guid.NewGuid();
            var fakeBookingDetail = new Faker<BookingDetail>()
                .RuleFor(_ => _.BookingId, bookingId).Generate();
            _bookingRepositoryMock.Setup(b => b.CreateAsync(fakeBookingDetail));

            var sut = new BookingService(_bookingRepositoryMock.Object);

            //Act
            var result = await sut.CreateBookingAsync(fakeBookingDetail.BookingStartTime, fakeBookingDetail.CustomerName);

            //Assert
            result.Should().NotBeNull();
            result.GetType().Should().Be(typeof(Guid));
        }

        [Theory]
        [InlineData(3, true)]
        [InlineData(2, true)]
        [InlineData(4, false)]
        [InlineData(5, false)]
        public async Task IsBookingTimeAvailable_Returns(int count, bool isBookingAvailable)
        {
            //Arrange
            var fakeBookingDetails = new Faker<BookingDetail>().Generate(count);
            _bookingRepositoryMock.Setup(b => b.GetByTimeAsync(It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>())).ReturnsAsync(fakeBookingDetails);

            var sut = new BookingService(_bookingRepositoryMock.Object);

            //Act
            var result = await sut.IsBookingTimeAvailable(It.IsAny<TimeSpan>());

            //Assert
            result.Should().Be(isBookingAvailable);
        }
    }
}
