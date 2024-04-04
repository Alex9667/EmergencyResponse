using EmergencyResponse.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmergencyResponse.Controller;

namespace EmergencyResponseTest
{
    public class AddressControllerTest
    {
        private AddressController _addressController;
        public AddressControllerTest(AddressController addressController) 
        {
            _addressController = addressController;
        }

        [Fact]
        public void AddressController_GetAddress_ShouldReturnData()
        {
            Address address = _addressController.GetAddress();

            address.Should().NotBeNull();   
        }
    }
}
