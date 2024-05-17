using System.Net;
using EmergencyResponse.Model;

namespace EmergencyResponseTest
{
    public class AddressModelTest
    {
        // [TestClass]
        public class APIValidatorTests
        {
            // private APIValidator _validator;

            /*[SetUp]
            public void Setup()
            {
                // Initialiser APIValidator før hver test
                _validator = new APIValidator();
            }*/

            [Theory]
            [InlineData("Oak Avenue", "1011", null, "D", "13579", "City D")]
            public void AddressProperties_ShouldMatchInputValues(string streetName, string houseNumber, string? floor, string door,
                string postalCode, string postalCodeName)
            {
                // Act
                var address = new Address(streetName, houseNumber, floor, door, postalCode, postalCodeName);

                // Assert
                Assert.Equal(streetName, address.StreetName);
                Assert.Equal(houseNumber, address.HouseNumber);
                Assert.Equal(floor, address.Floor);
                Assert.Equal(door, address.Door);
                Assert.Equal(postalCode, address.PostalCode);
                Assert.Equal(postalCodeName, address.PostalCodeName);
            }


            public static IEnumerable<object[]> GetAddressData()
            {
                yield return new object[]
                {
                null, null, null, null, null, null, null
                };
            }

            [Theory, MemberData(nameof(GetAddressData))]
            public void AddressConstructor_WithNullValuesForNonNullableProperties_ShouldThrowArgumentNullException(string streetName, string houseNumber, string? floor, string door,
                string postalCode, string postalCodeName)
            {
                // Act
                Action action = () => new Address(streetName, houseNumber, floor, door, postalCode, postalCodeName);

                // Assert
                Assert.Throws<ArgumentNullException>(action);
            }

            [Fact]
            public void SetAddressId_WhenCalledFirstTime_SetsTheAddressId()
            {
                // Arrange
                var address = new Address("streetName", "houseNumber", null, "door", "postalCode", "postalCodeName");
                var addressId = "newAddressId";

                // Act
                Action act = () => address.SetAddressId(addressId);

                // Assert
                act.Should().NotThrow();
                address.AddressId.Should().Be(addressId);
            }

            [Fact]
            public void SetAddressId_ForTheSecondTime_ThrowsInvalidOperationException()
            {
                // Arrange
                var address = new Address("streetName", "houseNumber", null, "door", "postalCode", "postalCodeName");
                address.SetAddressId("firstUniqueId"); // First time setting AddressId

                // Act
                Action act = () => address.SetAddressId("secondUniqueId"); // Attempting to set again

                // Assert
                act.Should().Throw<InvalidOperationException>().WithMessage("AddressId can only be set once.");
            }

            [Theory]
            [InlineData(null, typeof(ArgumentNullException), "AddressId cannot be null. (Parameter 'addressId')")]
            public void SetAddressId_FailureScenarios_ThrowsExpectedException(string AddressId, Type expectedException, string expectedMessage)
            {
                // Arrange
                var address = new Address("streetName", "houseNumber", null, "door", "postalCode", "postalCodeName");

                // Act
                Action act = () => address.SetAddressId(AddressId);

                // Assert
                act.Should().Throw<Exception>().Where(e => e.GetType() == expectedException && e.Message.Contains(expectedMessage));
            }
        }
    }
}