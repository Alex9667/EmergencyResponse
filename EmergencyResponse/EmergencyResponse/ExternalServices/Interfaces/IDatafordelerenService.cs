using EmergencyResponse.DTO;
using EmergencyResponse.Model;

namespace EmergencyResponse.ExternalServices.Interfaces
{
    public interface IDatafordelerenService
    {
        Task<List<AddressDTO>> GetAddressesInBuilding(AddressDTO address);
        Task<string> GetJordstykkeFromDAR(string addressId);
        Task<string> GetGrundIdFromBBR(string jordstykke);
    }
}
