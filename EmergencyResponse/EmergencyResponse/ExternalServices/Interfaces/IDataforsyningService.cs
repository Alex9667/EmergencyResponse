﻿using EmergencyResponse.DTO;
using EmergencyResponse.Model;

namespace EmergencyResponse.ExternalServices.Interfaces
{
    public interface IDataforsyningService
    {
        Task<Address> GetAddressAsync(Address address);
        Task<List<Address>> GetAddressesInCircleAsync(Address center, int radius);
        Task<List<AddressDTO>> SearchAddress(AddressDTO address);
    }
}
