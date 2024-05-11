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
        public string PostalCodeName { get; set; }
        public string? AddressId { get; set; }
        public int? BFE { get; set; } = null;

        public AddressDTO(Address address)
        {
            StreetName = address.StreetName;
            HouseNumber = address.HouseNumber;
            Floor = address.Floor;
            Door = address.Door;
            PostalCode = address.PostalCode;
            PostalCodeName = address.PostalCodeName;
        }

        public AddressDTO()
        {
        }
    }
}
