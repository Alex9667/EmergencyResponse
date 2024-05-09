using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmergencyResponse.Model;
using FluentAssertions;
using Xunit;

namespace EmergencyResponseTest
{
    public class CircleTests
    {
        public static IEnumerable<object[]> AddressData()
        {
            var validAddress = new Address("1", "Main Street", "123", null, null, "12345", "SmallTown");
            validAddress.SetLatitude(34.0522);
            validAddress.SetLongitude(-118.2437);
            yield return new object[] { validAddress, false, null};

            var addressMissingLat = new Address("2", "Second Street", "456", null, null, "67890", "BigTown");
            addressMissingLat.SetLongitude(-118.2437);  // Only longitude is set
            yield return new object[] { addressMissingLat, true, "Address must have both latitude and longitude set." };

            var addressMissingLon = new Address("3", "Third Street", "789", null, null, "54321", "MidTown");
            addressMissingLon.SetLatitude(34.0522);  // Only latitude is set
            yield return new object[] { addressMissingLon, true, "Address must have both latitude and longitude set." };
        }

        [Theory]
        [MemberData(nameof(AddressData))]
        public void Circle_Constructor_ShouldValidateAddress(Address address, bool shouldThrow, string expectedMessage)
        {
            // Arrange
            Action act = () => new Circle(address);

            // Act & Assert
            if (shouldThrow)
            {
                var exception = Assert.Throws<InvalidOperationException>(act);
                Assert.Equal(expectedMessage, exception.Message);
            }
            else
            {
                Circle circle = new Circle(address);
                var circleException = Record.Exception(() => new Circle(address));
                Assert.Null(circleException);  // Ensure no exception is thrown
                Assert.Equal(address.Latitude, circle.Center.Latitude);
                Assert.Equal(address.Longitude, circle.Center.Longitude);
            }
        }
    }
}
