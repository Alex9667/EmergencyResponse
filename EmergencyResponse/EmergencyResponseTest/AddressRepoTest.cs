using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmergencyResponse.Interfaces;
using EmergencyResponse.Model;

namespace EmergencyResponseTest
{
    public class AddressRepoTest
    {
        private Mock<IAddressRepo> _addressRepo;
        public AddressRepoTest() 
        {
            _addressRepo = new Mock<IAddressRepo>();

            Address address = new("askda-2daskdl-209j", "Bornholmvej", "21A", 1, "5", "7892", "Bornholm");

            _addressRepo.Setup(x => x.GetAddress("Bornholmvej 21A")).Returns(address);
        }

        [Fact]
        public void GetAddress_ShouldGetAddress()
        {
            Address address = _addressRepo.Object.GetAddress("Bornholmvej 21A");

            address.Should().NotBeNull();
        }

    }
}
