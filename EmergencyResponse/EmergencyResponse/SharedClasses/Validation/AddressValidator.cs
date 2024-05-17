using EmergencyResponse.Model;
using EmergencyResponse.SharedClasses.Validation.Interfaces;
using System.Text.RegularExpressions;

namespace EmergencyResponse.SharedClasses.Validation
{
    public class AddressValidator : IAddressValidator
    {
        public void Validate(Address address)
        {
            // Validate StreetName
            if (string.IsNullOrWhiteSpace(address.StreetName))
            {
                throw new ArgumentException("Street name cannot be empty.", nameof(address.StreetName));
            }

            // Validate HouseNumber
            if (string.IsNullOrWhiteSpace(address.HouseNumber) || !Regex.IsMatch(address.HouseNumber, @"^\d{1,3}[A-Z]?$"))
            {
                throw new ArgumentException("House number must be a number from 1 to 999 with an optional capital letter from A-Z immediately following.", nameof(address.HouseNumber));
            }

            // Validate Floor and Door
            if ((address.Floor != null && string.IsNullOrWhiteSpace(address.Floor)) ||
                (address.Door != null && string.IsNullOrWhiteSpace(address.Door)))
            {
                throw new ArgumentException("Floor and door information, if provided, must not be empty.", nameof(address.Floor) + "/" + nameof(address.Door));
            }

            // Validate PostalCode
            if (string.IsNullOrWhiteSpace(address.PostalCode) || !Regex.IsMatch(address.PostalCode, @"^\d{4}$"))
            {
                throw new ArgumentException("Postal code must be exactly four digits.", nameof(address.PostalCode));
            }

            // Validate PostalCodeName
            if (string.IsNullOrWhiteSpace(address.PostalCodeName) || !Regex.IsMatch(address.PostalCodeName, @"^[A-Za-zæøåÆØÅ\- ]+$"))
            {
                throw new ArgumentException("Postal code name must only contain letters, spaces, and hyphens from the Danish alphabet.", nameof(address.PostalCodeName));
            }
        }
    }
}
