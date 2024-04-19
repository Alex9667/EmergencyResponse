using EmergencyResponse.Model;

namespace EmergencyResponse.Interfaces
{
    public interface IAddressRepo
    {
        public void CreateAddress(string address);

        public Address GetAddress(string address);

        public Address UpdateAddress(string id, string streetName, string houseNumber, int? floor, string door,
                string postalCode, string postalCodeName);

        public void DeleteAddress(int id);
    }
}
