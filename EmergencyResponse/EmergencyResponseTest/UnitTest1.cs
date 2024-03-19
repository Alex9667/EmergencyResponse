using System.Net;
using EmergencyResponse.Model;
using Xunit;

namespace EmergencyResponseTest
{
    public class UnitTest1
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
            [InlineData("0a31asdac-2mnsb-3218-e042-0003ba293131", "Oak Avenue", "1011", null, "D", "13579", "City D", "addr4")]
            public void Create_Address(string id, string streetName, string houseNumber, int? floor, string door,
                string postalCode, string postalCodeName, string addressId)
            {
                //Arrange
                Address result = new Address
                {
                    Id = "0a31asdac-2mnsb-3218-e042-0003ba293131",
                    StreetName = "Oak Avenue",
                    HouseNumber = "1011",
                    Floor = null,
                    Door = "D",
                    PostalCode = "13579",
                    PostalCodeName = "City D"
                };

                //Act
                Address expectedResult = new Address
                {
                    Id = id,
                    StreetName = streetName,
                    HouseNumber = houseNumber,
                    Floor = floor,
                    Door = door,
                    PostalCode = postalCode,
                    PostalCodeName = postalCodeName,
                    AddressId = addressId
                };


                // Assert
                expectedResult.Equals(result);
            }


            public static IEnumerable<object[]> GetAddressData()
            {
                yield return new object[]
                {
                null, null, null, null, null, null, null, null
                };
            }

            [Theory, MemberData(nameof(GetAddressData))]
            public void MemberDataAddition_ShouldCalculateCorrectly(string id, string streetName, string houseNumber, int? floor, string door,
                string postalCode, string postalCodeName, string addressId)
            {
                //Act
                Action act = () => {
                    Address expectedResult = new Address
                    {
                        Id = id,
                        StreetName = streetName,
                        HouseNumber = houseNumber,
                        Floor = floor,
                        Door = door,
                        PostalCode = postalCode,
                        PostalCodeName = postalCodeName,
                        AddressId = addressId
                    };
                };

                var exception = Record.Exception(act);


                // Assert
                Assert.Throws<NullReferenceException>(act);
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