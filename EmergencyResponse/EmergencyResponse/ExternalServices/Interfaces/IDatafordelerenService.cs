using EmergencyResponse.DTO;
using EmergencyResponse.Model;

namespace EmergencyResponse.ExternalServices.Interfaces
{
    public interface IDatafordelerenService
    {
        Task<List<Address>> GetAddressesInBuilding(Address address);
        Task<string> GetJordstykkeFromDAR(string addressId);
        Task<string> GetGrundIdFromBBR(string jordstykke);
    }
}
