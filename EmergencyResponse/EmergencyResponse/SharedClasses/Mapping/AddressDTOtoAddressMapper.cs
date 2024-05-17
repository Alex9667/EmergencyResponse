using EmergencyResponse.DTO;
using EmergencyResponse.Model;

namespace EmergencyResponse.SharedClasses.Mapping
{
    public static class AddressDTOtoAddressMapper
    {
        // Convert AddressDTO to Address
        public static Address ToAddress(AddressDTO addressDTO)
        {
            if (addressDTO == null)
            {
                throw new ArgumentNullException(nameof(addressDTO), "AddressDTO cannot be null.");
            }

            var address = new Address(
                addressDTO.StreetName,
                addressDTO.HouseNumber,
                addressDTO.Floor,
                addressDTO.Door,
                addressDTO.PostalCode,
                addressDTO.PostalCodeName,
                addressDTO.AddressId
            );

            if (addressDTO.Latitude.HasValue)
            {
                address.SetLatitude(addressDTO.Latitude);
            }

            if (addressDTO.Longitude.HasValue)
            {
                address.SetLongitude(addressDTO.Longitude);
            }

            return address;
        }

        // Convert Address to AddressDTO
        public static AddressDTO ToAddressDTO(Address address)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address), "Address cannot be null.");
            }

            return new AddressDTO
            {
                StreetName = address.StreetName,
                HouseNumber = address.HouseNumber,
                Floor = address.Floor,
                Door = address.Door,
                PostalCode = address.PostalCode,
                PostalCodeName = address.PostalCodeName,
                AddressId = address.AddressId,
                Latitude = address.Latitude,
                Longitude = address.Longitude
            };
        }
    }
}
