using EmergencyResponse.Model;

namespace EmergencyResponse.ExternalServices.Interfaces
{
    public interface IDataforsyningService
    {
        Task<Address> GetAddressAsync(Address address);
    }
}
