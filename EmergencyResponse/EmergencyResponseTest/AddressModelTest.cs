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
                // Initialiser APIValidator f�r hver test
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



            /*

                        [TestMethod]
                        public void ValidateAddress_ShouldReturnTrue_ForValidAddress()
                        {
                            // Arrange
                            var validAddress = new AddressDTO("ValidStreet", "ValidCity", "1234");

                            // Act
                            var result = _validator.ValidateAddress(validAddress);

                            // Assert
                            Assert.IsTrue(result, "Expected validation to pass for a valid address.");
                        }

                        [TestMethod]
                        public void ValidateAddress_ShouldReturnFalse_ForInvalidZipCode()
                        {
                            // Arrange
                            var addressWithInvalidZip = new AddressDTO("ValidStreet", "ValidCity", "InvalidZip");

                            // Act
                            var result = _validator.ValidateAddress(addressWithInvalidZip);

                            // Assert
                            Assert.IsFalse(result, "Expected validation to fail for an invalid zip code.");
                        }

                        [TestMethod]
                        public void ValidateAddress_ShouldReturnFalse_ForEmptyStreet()
                        {
                            // Arrange
                            var addressWithEmptyStreet = new AddressDTO("", "ValidCity", "1234");

                            // Act
                            var result = _validator.ValidateAddress(addressWithEmptyStreet);

                            // Assert
                            Assert.IsFalse(result, "Expected validation to fail for an empty street.");
                        }

                        [TestMethod]
                        public void ValidateAddress_ShouldReturnFalse_ForEmptyCity()
                        {
                            // Arrange
                            var addressWithEmptyCity = new AddressDTO("ValidStreet", "", "1234");

                            // Act
                            var result = _validator.ValidateAddress(addressWithEmptyCity);

                            // Assert
                            Assert.IsFalse(result, "Expected validation to fail for an empty city.");
                        }

                        [TestMethod]
                        public void ValidateAddress_ShouldReturnFalse_ForEmptyZipCode()
                        {
                            // Arrange
                            var addressWithEmptyZip = new AddressDTO("ValidStreet", "ValidCity", "");

                            // Act
                            var result = _validator.ValidateAddress(addressWithEmptyZip);

                            // Assert
                            Assert.IsFalse(result, "Expected validation to fail for an empty zip code.");
                        }
            */
        }
    }
}