using EmergencyResponse.Model;

namespace EmergencyResponse.Interfaces
{
    public interface IAddressRepo
    {
        public Address CreateAddress(string address);

        public Address GetAddress(string address);

        public Address UpdateAddress(string streetName, string houseNumber, string? floor, string door,
                string postalCode, string postalCodeName);

        public void DeleteAddress();
    }
}
