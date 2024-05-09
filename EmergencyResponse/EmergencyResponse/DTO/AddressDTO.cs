using EmergencyResponse.Model;

namespace EmergencyResponse.DTO
{
    public class AddressDTO
    {
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public string? Floor { get; set; }
        public string? Door { get; set; }
        public string PostalCode { get; set; }
        public string? PostalCodeName { get; set; }
        public string? AddressId { get; set; }

        public AddressDTO(Address address)
        {
            StreetName = address.StreetName;
            HouseNumber = address.HouseNumber;
            Floor = address.Floor;
            Door = address.Door;
            PostalCode = address.PostalCode;
            PostalCodeName = address.PostalCodeName;
            AddressId = address.AddressId;
        }
        public AddressDTO(string streetName, string houseNumber, string? floor, string? door, string postalCode, string? postalCodeName, string? addressId = null)
        {
            StreetName = streetName;
            HouseNumber = houseNumber;
            Floor = floor;
            Door = door;
            PostalCode = postalCode;
            PostalCodeName = postalCodeName;
            AddressId = addressId;
        }
        public AddressDTO()
        {
        }
    }
}
