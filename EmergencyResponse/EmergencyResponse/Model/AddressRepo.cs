using EmergencyResponse.Interfaces;
namespace EmergencyResponse.Model
{
    public class AddressRepo : IAddressRepo
    {
        public Address CreateAddress(string address)
        {
            throw new NotImplementedException();
        }

        public Address GetAddress(string address)
        {
            throw new NotImplementedException();
        }

        public Address UpdateAddress(string id, string streetName, string houseNumber, int? floor, string door,
                                  string postalCode, string postalCodeName)
        {
            throw new NotImplementedException();
        }

        public void DeleteAddress(int id)
        {
            throw new NotImplementedException();
        }
    }
}
