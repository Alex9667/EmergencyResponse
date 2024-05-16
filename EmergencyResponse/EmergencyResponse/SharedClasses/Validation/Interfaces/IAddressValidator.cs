using EmergencyResponse.Model;

namespace EmergencyResponse.SharedClasses.Validation.Interfaces
{
    public interface IAddressValidator
    {
        void Validate(Address address);
    }
}
