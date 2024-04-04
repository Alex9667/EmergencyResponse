using EmergencyResponse.Model;

namespace EmergencyResponse.Interfaces
{
    public interface IAddressRepo
    {
        public void CreateAddress(string address);

        public Address GetAddress(string address);

        public void UpdateAddress(int id);

        public void DeleteAddress(int id);
    }
}
