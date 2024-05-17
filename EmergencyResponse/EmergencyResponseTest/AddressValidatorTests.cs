using EmergencyResponse.Model;
using EmergencyResponse.SharedClasses.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmergencyResponseTest
{
    public class AddressValidatorTests
    {
        public static IEnumerable<object[]> AddressValidationData()
        {
            // Valid case
            yield return new object[]
            {
            new Address("Elm Street", "123A", "1", "2", "1000", "Copenhagen"),
            null,
            null // No exception expected
            };

            // Invalid street name cases
            yield return new object[]
            {
            new Address("", "123", "1", "2", "1000", "Copenhagen"),
            typeof(ArgumentException),
            "Street name cannot be empty. (Parameter 'StreetName')"
            };

            // Invalid house number cases
            yield return new object[]
            {
            new Address("Elm Street", "10000", "1", "2", "1000", "Copenhagen"),
            typeof(ArgumentException),
            "House number must be a number from 1 to 999 with an optional capital letter from A-Z immediately following. (Parameter 'HouseNumber')"
            };

            yield return new object[]
            {
            new Address("Elm Street", "123B4", "1", "2", "1000", "Copenhagen"),
            typeof(ArgumentException),
            "House number must be a number from 1 to 999 with an optional capital letter from A-Z immediately following. (Parameter 'HouseNumber')"
            };

            // Invalid floor and door cases
            yield return new object[]
            {
            new Address("Elm Street", "123", "", "2", "1000", "Copenhagen"),
            typeof(ArgumentException),
            "Floor and door information, if provided, must not be empty. (Parameter 'Floor/Door')"
            };

            yield return new object[]
            {
            new Address("Elm Street", "123", "1", "", "1000", "Copenhagen"),
            typeof(ArgumentException),
            "Floor and door information, if provided, must not be empty. (Parameter 'Floor/Door')"
            };

            // Invalid postal code cases
            yield return new object[]
            {
            new Address("Elm Street", "123", "1", "2", "10000", "Copenhagen"),
            typeof(ArgumentException),
            "Postal code must be exactly four digits. (Parameter 'PostalCode')"
            };

            yield return new object[]
            {
            new Address("Elm Street", "123", "1", "2", "ABC", "Copenhagen"),
            typeof(ArgumentException),
            "Postal code must be exactly four digits. (Parameter 'PostalCode')"
            };

            // Invalid postal code name cases
            yield return new object[]
            {
            new Address("Elm Street", "123", "1", "2", "1000", "København1"),
            typeof(ArgumentException),
            "Postal code name must only contain letters, spaces, and hyphens from the Danish alphabet. (Parameter 'PostalCodeName')"
            };

            yield return new object[]
            {
            new Address("Elm Street", "123", "1", "2", "1000", "New-York4"),
            typeof(ArgumentException),
            "Postal code name must only contain letters, spaces, and hyphens from the Danish alphabet. (Parameter 'PostalCodeName')"
            };
        }

        [Theory]
        [MemberData(nameof(AddressValidationData))]
        public void Validate_Address_ValidationRules(Address address, Type expectedExceptionType, string expectedMessage)
        {
            //Arrange
            var validator = new AddressValidator();

            //Act & Assert
            if (expectedExceptionType == null)
            {
                Exception ex = Record.Exception(() => validator.Validate(address));
                Assert.Null(ex); // Ensure no exception is thrown for valid cases
            }
            else
            {
                var exception = Assert.Throws(expectedExceptionType, () => validator.Validate(address));
                Assert.Equal(expectedMessage, exception.Message); // Assert that the correct message is thrown
            }
        }
    }
}
