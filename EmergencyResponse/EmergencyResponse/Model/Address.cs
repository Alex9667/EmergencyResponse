namespace EmergencyResponse.Model
{
    public class Address
    {
        public string Id { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public int? Floor { get; set; }
        public string? Door { get; set; }
        public string PostalCode { get; set; }
        public string PostalCodeName { get; set; }
        public string? AddressId { get; set; }

        public Address(string id, string streetName, string houseNumber, int? floor, string? door, string postalCode, string? postalCodeName, string? addressId = null)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id), "Id cannot be null.");
            StreetName = streetName ?? throw new ArgumentNullException(nameof(streetName), "StreetName cannot be null.");
            HouseNumber = houseNumber ?? throw new ArgumentNullException(nameof(houseNumber), "HouseNumber cannot be null.");
            Floor = floor; // Nullable, no check needed
            Door = door; // Nullable, no check needed
            PostalCode = postalCode ?? throw new ArgumentNullException(nameof(postalCode), "PostalCode cannot be null.");
            PostalCodeName = postalCodeName ?? throw new ArgumentNullException(nameof(postalCodeName), "PostalCodeName cannot be null.");
            AddressId = addressId;
        }

        public Address()
        {
        }

        public void SetAddressId(string addressId)
        {
            if (AddressId != null)
            {
                throw new InvalidOperationException("AddressId can only be set once.");
            }

            AddressId = addressId ?? throw new ArgumentNullException(nameof(addressId), "AddressId cannot be null.");
        }
    }
}
