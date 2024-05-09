namespace EmergencyResponse.DTO
{
    public class AddressDTO
    {
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public int? Floor { get; set; }
        public string? Door { get; set; }
        public string PostalCode { get; set; }
        public string PostalCodeName { get; set; }
        public string? AddressId { get; set; }

    }
}
