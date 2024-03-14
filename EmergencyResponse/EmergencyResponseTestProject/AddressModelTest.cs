using System.Net.Sockets;
using EmergencyResponse.Model;

namespace EmergencyResponseTestProject
{
    [TestClass]
    public class APIValidatorTests
    {
        private APIValidator _validator;

        [SetUp]
        public void Setup()
        {
            // Initialiser APIValidator før hver test
            _validator = new APIValidator();
        }

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
}