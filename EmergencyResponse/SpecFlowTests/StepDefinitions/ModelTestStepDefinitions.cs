using EmergencyResponse.Model;

namespace SpecFlowTests.StepDefinitions
{
    [Binding]
    public sealed class ModelTestStepDefinitions
    {
        private Address _address;
        private string? _streetName;
        private string? _houseNumber;
        private string? _floor;
        private string? _door;
        private string? _postalCode;
        private string? _postalCodeName;

        [Given("the street name is (.*)")]
        public void GivenStreetNameIs(string streetName)
        {
            _streetName = streetName;
        }

        [Given("the housenumber is (.*)")]
        public void GivenTheHouseNumberIs(string houseNumber)
        {
            //arrange (precondition)
            _houseNumber = houseNumber;
        }

        [Given("the floor is (.*)")]
        public void GivenTheFloorIs(string floor)
        {
            //arrange (precondition)
            _floor = floor;
        }

        [Given("the door is (.*)")]
        public void GivenTheDoorIs(string door)
        {
            //arrange (precondition)
            _door = door;
        }

        [Given("the postalcode is (.*)")]
        public void GivenThePostalCodeIs(string postalCode)
        {
            //arrange (precondition)
            _postalCode = postalCode;
        }

        [Given("the postal code name is (.*)")]
        public void GivenThePostalCodeNameIs(string postalCodeName)
        {
            //arrange (precondition)
            _postalCodeName = postalCodeName;
        }

        [When("the address is created")]
        public void WhenTheAddressIsCreated()
        {
            //act (action)
            _address = new Address(_streetName, _houseNumber, _floor, _door, _postalCode, _postalCodeName);
        }

        [Then("the address should match the inputs")]
        public void ThenTheAddressShouldMatchTheInputs()
        {
            //assert (verification) 
            _address.StreetName.Should().BeEquivalentTo(_streetName);
            _address.HouseNumber.Should().BeEquivalentTo(_houseNumber);
            _address.Floor.Should().BeEquivalentTo(_floor);
            _address.Door.Should().BeEquivalentTo(_door);
            _address.PostalCode.Should().BeEquivalentTo(_postalCode);
            _address.PostalCodeName.Should().BeEquivalentTo(_postalCodeName);
        }
    }
}
