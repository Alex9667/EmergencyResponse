using EmergencyResponse.DTO;
using EmergencyResponse.Model;

namespace EmergencyResponse.ExternalServices.Interfaces
{
    public interface IDatafordelerenService
    {
        Task<List<AddressDTO>> GetAddressesInBuilding(AddressDTO address);
        Task<string> GetJordstykkeFromDAR(string addressId);
        Task<string> GetHusnummerIdFromDAR(string addressId);
        Task<string> GetGrundIdFromBBR(string jordstykke);
        Task<string> GetBuildingIdFromBBR(string grundId, string husnummerId);
        Task<List<string>> GetUnitsIdsFromBBR(string buildingId);
        Task<List<AddressDTO>> GetUnitAddressesFromDAR(List<string> unitIds);
        Task<string> CallApi(string url);
        Task<string> CallDARAddressApi(string addressId);
    }
}
