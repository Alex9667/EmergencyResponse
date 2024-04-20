using EmergencyResponse.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmergencyResponse.Controller;
using System.Net.Http;


namespace EmergencyResponseTest
{
    public class AddressControllerTest
    {
        private readonly AddressController _addressController;

        public AddressControllerTest()
        {
            HttpClient httpClient = new HttpClient(); 
            _addressController = new AddressController(httpClient);
        }

        [Fact]
        public void AddressController_GetAddress_ShouldReturnData()
        {
            Address address = _addressController.GetAddress();
            address.Should().NotBeNull();   
        }

        [Theory]
        [InlineData("Nygade 74 7430 Ikast")]
        public void AddressController_GetAddressBFE_ShouldReturnString(string address)
        {
            Task<int> bfe = _addressController.GetAddressBFE(address);
            bfe.ToString().Should().NotBeNullOrEmpty();
        }
    }
}
