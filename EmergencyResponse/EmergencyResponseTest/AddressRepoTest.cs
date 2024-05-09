using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmergencyResponse.Interfaces;
using EmergencyResponse.Model;
using System.Net;

namespace EmergencyResponseTest
{
    public class AddressRepoTest
    {
        private Mock<IAddressRepo> _addressRepoMock;
        private AddressRepo _addressRepo;

        public AddressRepoTest() 
        {
            _addressRepo = new AddressRepo();
            _addressRepoMock = new Mock<IAddressRepo>();

            Address address = new("askda-2daskdl-209j", "Bornholmvej", "21A", 1, "5", "7892", "Bornholm");

            _addressRepoMock.Setup(x => x.GetAddress("Bornholmvej 21A")).Returns(address);
        }

        [Fact]
        public void GetAddress_ShouldGetAddress()
        {
            Address address = _addressRepoMock.Object.GetAddress("Bornholmvej 21A");

            address.Should().NotBeNull();
        }

        [Fact]
        public void UpdateAddress_ShouldChangeAddressProperties()
        {
            Address oldAddress = new("askda-2daskdl-209j", "Bornholmvej", "21A", 1, "5", "7892", "Bornholm");

            Address newAddress = _addressRepo.UpdateAddress(oldAddress.Id, "Nygade", oldAddress.HouseNumber, oldAddress.Floor,
                                                            oldAddress.Door, oldAddress.PostalCode, oldAddress.PostalCodeName);

            newAddress.Should().NotBe(oldAddress);
        }

    }
}
