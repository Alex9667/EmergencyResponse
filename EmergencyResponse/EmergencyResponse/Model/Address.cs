namespace EmergencyResponse.Model
{
    public class Address
    {
        public string Id { get; private set; }
        public string StreetName { get; private set; }
        public string HouseNumber { get; private set; }
        public int? Floor { get; private set; }
        public string? Door { get; private set; }
        public string PostalCode { get; private set; }
        public string PostalCodeName { get; private set; }
        public string? AddressId { get; private set; }
        public double? Latitude { get; private set; }
        public double? Longitude { get; private set; }

        public Address(string id, string streetName, string houseNumber, int? floor, string door, string postalCode, string postalCodeName)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id), "Id cannot be null.");
            StreetName = streetName ?? throw new ArgumentNullException(nameof(streetName), "StreetName cannot be null.");
            HouseNumber = houseNumber ?? throw new ArgumentNullException(nameof(houseNumber), "HouseNumber cannot be null.");
            Floor = floor; // Nullable, no check needed
            Door = door; // Nullable, no check needed
            PostalCode = postalCode ?? throw new ArgumentNullException(nameof(postalCode), "PostalCode cannot be null.");
            PostalCodeName = postalCodeName ?? throw new ArgumentNullException(nameof(postalCodeName), "PostalCodeName cannot be null.");
        }

        public void SetAddressId(string addressId)
        {
            if (AddressId != null)
            {
                throw new InvalidOperationException("AddressId can only be set once.");
            }

            AddressId = addressId ?? throw new ArgumentNullException(nameof(addressId), "AddressId cannot be null.");
        }

        public void SetLatitude(double? latitude)
        {
            if (Latitude != null)
            {
                throw new InvalidOperationException("Latitude can only be set once.");
            }

            Latitude = latitude ?? throw new ArgumentNullException(nameof(latitude), "Latitude cannot be null.");
        }

        public void SetLongitude(double? longitude)
        {
            if (Longitude != null)
            {
                throw new InvalidOperationException("Longitude can only be set once.");
            }

            Longitude = longitude ?? throw new ArgumentNullException(nameof(longitude), "Longitude cannot be null.");
        }
    }
}
